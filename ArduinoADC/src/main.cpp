
#include "Arduino.h"

#include "SPI.h"

#define VERSION "1.0.0b"
// #define TEST_DATA

// ADS1220 SPI commands
#define SPI_MASTER_DUMMY    0xFF
#define RESET               0x06   //Send the RESET command (06h) to make sure the ADS1220 is properly reset after power-up
#define START               0x08    //Send the START/SYNC command (08h) to start converting in continuous conversion mode
#define WREG                0x40
#define RREG                0x20

// Config registers
#define CONFIG_REG0_ADDRESS 0x00
#define CONFIG_REG1_ADDRESS 0x01
#define CONFIG_REG2_ADDRESS 0x02
#define CONFIG_REG3_ADDRESS 0x03

uint8_t m_config_reg0;
uint8_t m_config_reg1;
uint8_t m_config_reg2;
uint8_t m_config_reg3;

uint8_t Config_Reg0;
uint8_t Config_Reg1;
uint8_t Config_Reg2;
uint8_t Config_Reg3;

uint8_t drdy_pin=2;
uint8_t cs_pin=7;

#define   PGA 1                 // Programmable Gain = 1
#define   VREF 2.048            // Internal reference of 2.048V
#define   VFSR VREF/PGA
#define   FSR (((long int)1<<23)-1)
int32_t   mResult32;
float     Vout;
float     mV_URV = 58.032;
float     OUT_IN_PERCENTS;


// Measurement states
#define MSTATE_IDLE 0
#define MSTATE_RUNNING 1

uint8_t state = MSTATE_IDLE;

#define PREBUFFER_SIZE 3
float preBuffer[PREBUFFER_SIZE] = { 0 };
int preBufferPos = 0;

void ads1220_Reset();
void readSerial();
bool checkInputSamples();
void Read_ADC_Data();
void writeRegister(uint8_t address, uint8_t value);
uint8_t readRegister(uint8_t address);
void Start_Conv();


void setup()
{
    pinMode(cs_pin, OUTPUT);
    pinMode(drdy_pin, INPUT_PULLUP);  

    Serial.begin(9600);

    SPI.begin();
    SPI.setBitOrder(MSBFIRST);
    SPI.setDataMode(SPI_MODE1);

    digitalWrite(cs_pin,LOW);
    delay(100);
    ads1220_Reset();
    delay(100);

    m_config_reg0 = B00000000;    //AINP=AIN0, AINN=AIN1, Gain 1, PGA enabled
    m_config_reg1 = B10000100;    //DR=330 SPS, Mode=Normal, Conv mode=continuous, Temp Sensor disabled, Current Source off
    m_config_reg2 = B00010000;    //Default settings: Vref internal, 50/60Hz rejection, low-side switch open, IDAC off
    m_config_reg3 = B00000000;    //Default settings: IDAC1 disabled, IDAC2 disabled, DRDY pin only

    writeRegister( CONFIG_REG0_ADDRESS , m_config_reg0);
    writeRegister( CONFIG_REG1_ADDRESS , m_config_reg1);
    writeRegister( CONFIG_REG2_ADDRESS , m_config_reg2);
    writeRegister( CONFIG_REG3_ADDRESS , m_config_reg3);

    delay(100);

    Config_Reg0 = readRegister(CONFIG_REG0_ADDRESS);
    Config_Reg1 = readRegister(CONFIG_REG1_ADDRESS);
    Config_Reg2 = readRegister(CONFIG_REG2_ADDRESS);
    Config_Reg3 = readRegister(CONFIG_REG3_ADDRESS);
    
    digitalWrite(cs_pin,HIGH);
    
    delay(100);
    
    Start_Conv();   //Start continuous conversion mode
    delay(100);

    attachInterrupt(digitalPinToInterrupt(drdy_pin), Read_ADC_Data, FALLING);

#ifdef TEST_DATA
    delay(100);
    sendTestData();
#endif
}


#ifdef TEST_DATA
void sendTestData()
{
    static float testData[] = { 30.7, 40.9, 48.8, 53.9, 56.3, 57.1, 57.1, 56.3, 55.1, 54.7, 53.9, 53.2, 53.2, 53.2, 53.9, 54.7, 55.1, 55.5, 56.3,
    57.5, 58.3, 58.3, 58.3, 58.6, 58.6, 58.6, 58.6, 58.6, 58.6, 58.3, 58.3, 57.5, 57.5, 57.5, 57.5, 57.1, 57.1, 57.1,
    56.3, 56.3, 55.5, 55.5, 55.5, 55.5, 55.5, 55.1, 55.1, 54.7, 54.7, 54.7, 54.7, 54.7, 54.7, 54.7, 54.4, 54.4, 53.9,
    53.9, 53.9, 53.9, 53.9, 53.9, 53.2, 53.2, 52.3, 52.3, 52.3, 52.3, 52.3, 52.3, 52.3, 52.0, 52.0, 52.0, 51.1, 51.1,
    52.0, 52.0, 51.1, 51.1, 51.1, 51.1, 50.8, 51.1, 51.1, 51.1, 52.0, 52.0, 52.3, 53.2, 53.9, 54.7, 55.5, 57.1, 58.3,
    58.6, 59.5, 60.3, 60.7, 60.7, 61.4, 61.9, 61.9, 62.2, 62.2, 62.2, 62.2, 62.2, 62.6, 62.6, 62.6, 63.4, 63.4, 63.4,
    63.4, 62.6, 62.6, 62.6, 63.4, 63.4, 63.4, 63.4, 63.8, 63.4, 63.4, 63.4, 63.4, 63.8, 63.8, 63.8, 63.8, 63.8, 63.8,
    63.8, 63.8, 63.8, 63.8, 63.8, 63.8, 63.8, 63.8, 63.8, 63.8, 63.8, 63.4, 62.2, 60.3, 53.9, 22.0 };

    Serial.println("Beg");
    Serial.println("Bat:100% (4.51v)");

    for (int i=0; i < sizeof(testData)/sizeof(testData[0]); i++)
    {
        Serial.print("Val:");
        Serial.println(testData[i], 1);        
        delay(24);
    }

    Serial.println("End");
}
#endif


void loop()
{
    readSerial();

    switch (state) 
    {
    case MSTATE_IDLE:
        if (checkInputSamples())
        {
            Serial.println("Beg");
            Serial.println("Bat:100% (4.51v)");
            Serial.print("Val:");
            Serial.println(OUT_IN_PERCENTS, 1);

            memset(preBuffer, 0, sizeof(preBuffer));
            state = MSTATE_RUNNING;

            delay(24);
        } 
        else
        {
            preBuffer[preBufferPos] = OUT_IN_PERCENTS;            

            preBufferPos = (preBufferPos + 1) % PREBUFFER_SIZE;
            delay(10);
        }
        break;
    case MSTATE_RUNNING:
        Serial.print("Val:");
        Serial.println(OUT_IN_PERCENTS, 1);

        if (OUT_IN_PERCENTS <= 5.0f)
        {
            Serial.println("End");
            state = MSTATE_IDLE;
        }

        delay(24);
        break;
    }
}


bool checkInputSamples()
{
  for (int i=0 ; i < PREBUFFER_SIZE; i++)
  {
    if (preBuffer[i] < 5.0f)
    {
      return false;
    }
  }

  return true;
}


void readSerial()
{
    static char buff[32] = { 0 };
    static int  len = 0;

    int avail = Serial.available();

    // limit number of bytes to be read at once
    if (avail > 16) {
        avail = 16;
    }

    for (; avail > 0; avail--) {
        buff[len] = (uint8_t)Serial.read();

        if (buff[len] == '\n') 
        {
            if (strncmp(buff, "Batt?\r\n", len + 1) == 0) // report baterry level request
            {
                Serial.println("Beg");
                Serial.println("Bat:100% (4.51v)");
                Serial.println("End");
            }
            else if (strncmp(buff, "HVer?\r\n", len + 1) == 0) // report hardware version request
            {
                Serial.print("HVer:");
                Serial.println(VERSION);
            }

            len = 0;
        }
        else
        {
            len = (len + 1) % 32; // in case of too long (bad) message start overwiting bytes at the beginning
        }
    }
}


void Read_ADC_Data()
{
    static byte SPI_Buff[3];
    long int bit24;
    mResult32=0;

    digitalWrite(cs_pin, LOW);                                  //Take CS low
    delayMicroseconds(100);

    for (int i = 0; i < 3; i++)
    {
        SPI_Buff[i] = SPI.transfer(SPI_MASTER_DUMMY);
    }

    delayMicroseconds(100);
    digitalWrite(cs_pin, HIGH);                                //  Clear CS to high

    bit24 = SPI_Buff[0];
    bit24 = (bit24 << 8) | SPI_Buff[1];
    bit24 = (bit24 << 8) | SPI_Buff[2];                       // Converting 3 bytes to a 24 bit int

    bit24 = (bit24 << 8);
    mResult32 = (bit24 >> 8);                               // Converting 24 bit two's complement to 32 bit two's complement
    Vout = ((mResult32*VFSR*1000)/FSR);                       //In  mV
    
    if (Vout > mV_URV)
    {
        mV_URV = Vout;
    }

    OUT_IN_PERCENTS = (Vout/mV_URV)*100;
}



void writeRegister(uint8_t address, uint8_t value)
{
//    digitalWrite(cs_pin,LOW);
    delay(5);
    SPI.transfer(WREG|(address<<2));
    SPI.transfer(value);
    delay(5);
//    digitalWrite(cs_pin,HIGH);
}



uint8_t readRegister(uint8_t address)
{
    uint8_t data;

//    digitalWrite(cs_pin,LOW);
    delay(5);
    SPI.transfer(RREG|(address<<2));
    data = SPI.transfer(SPI_MASTER_DUMMY);
    delay(5);
//    digitalWrite(cs_pin,HIGH);

    return data;
}



void SPI_Command(unsigned char data_in)
{
    SPI.transfer(data_in);
    delay(10);
}


void ads1220_Reset()
{
    SPI_Command(RESET);
}


void Start_Conv()
{
    SPI_Command(START);
}
