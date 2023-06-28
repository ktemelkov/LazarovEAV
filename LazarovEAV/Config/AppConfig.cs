using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.Config
{
    /// <summary>
    /// 
    /// </summary>
    class AppConfig
    {
        public const string EAV_DEVICE_NAME = "BIO-BALANCE VOLL-TEST";
        public const string BIOCHECK_DEVICE_NAME = "USB biocheck3";
        public const string ARDUINO_DEVICE_NAME = "BIO-BALANCE ARDUINO";

        public const double EAV_WINDOW_BEGIN = 40.0;
        public const double EAV_WINDOW_END = 60.0;

        public const string APPLICATION_NAME = "LazarovEAV";

        public const int TEST_TABLE_POSITIONS = 8;
        public const int MIX_TEST_POSITIONS = 3;
        public const int MIX_TEST_SLOTS = 1;
        public const int SUBSTANCE_TEST_SLOTS = 5;
        public const int SUBSTANCE_TEST_PAGES = 6;

        public static double SUBSTANCE_TEST_SLOT_POSITIONS { get { return AppConfig.TEST_TABLE_POSITIONS + 1; } }
        public static double SUBSTANCE_TEST_GRAPH_POSITIONS { get { return AppConfig.SUBSTANCE_TEST_SLOT_POSITIONS * AppConfig.SUBSTANCE_TEST_SLOTS + (AppConfig.MIX_TEST_POSITIONS + 1) * AppConfig.MIX_TEST_SLOTS; } }

        public static List<string> PotencyList = new List<string>()
        {
            "D6", "D10", "D12", "D15", "D26", "D30", "D60", "D200",
            "---", "6CH", "9CH", "15CH", "30CH", "200CH",
            "---", "1M", "10M", "50M",
            "---", "LM1", "LM2", "LM3", "LM4", "LM5",
            "---", "CM",
        };


        public static string APP_DATA_PATH
        {
            get
            {
                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Data");

                if (!Directory.Exists(path))
                {
                    try
                    {
                        Directory.CreateDirectory(path);
                    }
                    catch (Exception)
                    {
                        path = Path.GetTempPath();
                    }
                }
                
                return path;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public static string SUBST_IMAGE_PATH
        {
            get             
            {
                string path = Path.Combine(AppConfig.APP_DATA_PATH, "Images");

                if (!Directory.Exists(path))
                {
                    try
                    {
                        Directory.CreateDirectory(path);
                    }
                    catch (Exception)
                    {
                        path = Path.GetTempPath();
                    }
                }

                return path;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public static string SUBST_IMAGE_CONFIG_FILENAME
        {
            get
            {
                return Path.Combine(AppConfig.SUBST_IMAGE_PATH, "images.json");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public static string PALETTE_CONFIG_FILENAME
        {
            get
            {
                return Path.Combine(AppConfig.APP_DATA_PATH, "uiconfig.json");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public static string UISTATE_FILENAME
        {
            get
            {
                return Path.Combine(AppConfig.APP_DATA_PATH, "uistate.json");
            }
        }
    }
}
