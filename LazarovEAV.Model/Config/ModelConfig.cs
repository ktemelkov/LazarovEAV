using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.Model.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class ModelConfig
    {
        public const string APPLICATION_NAME = "LazarovEAV";
        public const string DATABASE_FILENAME = APPLICATION_NAME + ".db3";


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


        public static string DATABASE_PATH
        { 
            get {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                path = Path.Combine(path, ModelConfig.APPLICATION_NAME);
                
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                path = Path.Combine(path, ModelConfig.DATABASE_FILENAME);

                //if (!File.Exists(path))
                {
                    string dbPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    dbPath = Path.Combine(dbPath, "SQLite");
                    dbPath = Path.Combine(dbPath, "application.db3");

                //    File.Copy(dbPath, path);
                    path = dbPath;
                }

                return path;
            } 
        }
    }
}
