using LazarovEAV.Util;
using LazarovEAV.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LazarovEAV.Config
{
    /// <summary>
    /// 
    /// </summary>
    class UiState : NotifyPropertyChangedImpl
    {
        private static UiState instance;
        public static UiState Instance { get { return UiState.instance; } }


        [JsonProperty]
        public int selectedMeridianIndex = 0;

        [JsonProperty]
        public int selectedMeridianPointIndex = 0;

        [JsonProperty]
        private int liveViewSelectedTab = 1;

        [JsonIgnore]
        public int LiveViewSelectedTab { get { return this.liveViewSelectedTab; } set { this.liveViewSelectedTab = value; RaisePropertyChanged("LiveViewSelectedTab"); } }


        // test mode
        // selected pathogen
        // selected cure
        // selected hand

        // Window size and position
        // Live view tab control

        // history graph

        /// <summary>
        /// 
        /// </summary>
        static UiState()
        {
            string filename = AppConfig.UISTATE_FILENAME;
            
            UiState instance;

            if (!File.Exists(filename))
            {
                instance = new UiState();                

                try
                {
                    string json = JsonConvert.SerializeObject(instance, Formatting.Indented);
                    File.WriteAllText(filename, json);
                }
                catch (Exception)
                { }
            }
            else
            {
                instance = JsonConvert.DeserializeObject<UiState>(File.ReadAllText(filename));
            }

            UiState.instance = instance;
        }


        /// <summary>
        /// 
        /// </summary>
        private UiState()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            string filename = AppConfig.UISTATE_FILENAME;

            try
            {
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(filename, json);
            }
            catch (Exception e)
            {
                System.Console.Out.Write(e.StackTrace);
            }
        }
    }
}
