using LazarovEAV.Config;
using LazarovEAV.Device;
using LazarovEAV.Model;
using LazarovEAV.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;


namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    class MainViewModel : NotifyPropertyChangedImpl, IDisposable
    {
        private Window appWindow;

        private EavDeviceManager deviceManager;
        private ProtocolAdapter protocolAdapter;

        private Status status = new Status();
        public Status Status { get { return this.status; } }

        private Console console = new Console();
        public Console Console { get { return this.console; } }

        private PatientViewModel activePatient;
        public PatientViewModel ActivePatient 
        { 
            get { return this.activePatient; } 
            
            set 
            {
                if (this.activePatient != null)
                {
                    if (value == null || this.activePatient.Id != value.Id || this.activePatient.CurrentSession.Id != value.CurrentSession.Id)
                    {
                        this.activePatient.SaveCurrentSession();
                    }
                }

                this.activePatient = value; 
                RaisePropertyChanged("ActivePatient");
            } 
        }

        private EavDeviceViewModel eavDevice;
        public EavDeviceViewModel EavDevice { get { return this.eavDevice; } set { this.eavDevice = value; RaisePropertyChanged("EavDevice"); } }

        List<MeridianViewModel> meridians;
        public List<MeridianViewModel> Meridians { get { return this.meridians; } set { this.meridians = value; RaisePropertyChanged("Meridians"); } }

        private ICommand saveSessionCommand;
        public ICommand SaveSessionCommand { get { return this.saveSessionCommand; } }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="wnd"></param>
        public MainViewModel(Window wnd)
        {
            this.saveSessionCommand = new CommandDelegate(new Action<object>(saveSession));

            this.appWindow = wnd;
            this.appWindow.Closing += onApplicationClosing;
            this.appWindow.Loaded += (s, e) => { this.deviceManager.listDevices(onDeviceList); };

            this.eavDevice = new EavDeviceViewModel(this.protocolAdapter = new ProtocolAdapter(this.deviceManager = new EavDeviceManager(new DeviceLogCallback(this.deviceLogger))));

            initData();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void deviceLogger(string message)
        {
            printLog(message);
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (this.activePatient != null)
                this.activePatient.SaveCurrentSession();

            this.eavDevice.Dispose();
        }


        /// <summary>
        /// 
        /// </summary>
        private void initData()
        {
            using (EntityManager em = new EntityManager())
            {
                this.Meridians = (from m in em.loadItems<MeridianInfo>().OrderBy(mi => mi.SortKey) where m.Visible select new MeridianViewModel(m)).ToList();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void searchForDevice()
        {
            System.Timers.Timer t = new System.Timers.Timer(2000);
            t.AutoReset = false;

            t.Elapsed += (s, e) =>
            {
                this.appWindow.Dispatcher.BeginInvoke((Action)(() =>
                {
                    this.deviceManager.listDevices(onDeviceList);
                }), null);
            };

            t.Start();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="devices"></param>
        private void onDeviceList(List<EavDeviceInfo> devices)
        {
            if (devices != null && devices.Count > 0)
            {
                foreach (EavDeviceInfo dev in devices)
                {
                    printLog("Found suitable device.");
                    printLog(" Name: " + dev.Description);

                    connect(dev);
                    return;
                }
            }

            printLog("No suitable device found!");
            setStatusMessage("Не е открит измервателен уред.", StatusIconType.ERROR);

            searchForDevice();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dev"></param>
        private void connect(EavDeviceInfo dev)
        {
            printLog("Conecting ...");
            setStatusMessage("Открит е измервателен уред. Свързване ...", StatusIconType.ALERT);

            this.deviceManager.connect(dev, onConnect, onDisconnect, onDataReceived);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fSuccess"></param>
        private void onConnect(int errorCode)
        {
            if (errorCode == 0)
            {
                printLog("Connected.");
                setStatusMessage("Готов за работа с измервателния уред.", StatusIconType.OK);
            }
            else
            {
                printLog($"Connection failed! Code: { errorCode }");
                setStatusMessage("Неуспешна връзка с измервателния уред!", StatusIconType.ERROR);

                searchForDevice();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void onDisconnect()
        {
            printLog("Disconnected.");
            setStatusMessage("Прекратена връзка с измервателния уред.", StatusIconType.ALERT);

            if (!this.appWindow.IsEnabled)
            {
                this.appWindow.Close();
            }
            else
            {
                searchForDevice();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void onDataReceived(object data, DEVICE_TYPE devType)
        {
            this.protocolAdapter.feedData(data, devType);
//            printLog(data);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void printLog(string message)
        {
            this.Console.print(message);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void setStatusMessage(string message, StatusIconType t)
        {
            this.Status.Message = message;
            this.Status.Icon = t;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onApplicationClosing(object sender, CancelEventArgs e)
        {
            if (this.deviceManager.IsConnected)
            {
                e.Cancel = true;
                this.appWindow.IsEnabled = false;
                this.deviceManager.disconnect();
            }
            else
            {
                UiState.Instance.Save();
                this.Dispose();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void saveSession(object obj)
        {
            if (this.activePatient != null)
                this.activePatient.SaveCurrentSession();
        }
    }
}
