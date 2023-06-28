using LazarovEAV.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LazarovEAV.Config
{
    /// <summary>
    /// 
    /// </summary>
    class UiConfig : NotifyPropertyChangedImpl
    {
        private static UiConfig instance;
        public static UiConfig Instance { get { return UiConfig.instance; } }

        
        [JsonProperty] private Color normalDataColor = Color.FromArgb(255, 116, 116, 116);
        [JsonProperty] private Color pathogenDataColor = Color.FromArgb(255, 100, 150, 255);
        [JsonProperty] private Color cureDataColor = Color.FromArgb(255, 255, 100, 255);

        [JsonProperty] private Color baseDataColor = Color.FromArgb(255, 120, 120, 255);
        [JsonProperty] private Color selectedDataColor = Color.FromArgb(255, 255, 0, 128);

        [JsonProperty] private Color liveGraphNormalDataColor = Color.FromArgb(255, 116, 116, 116);
        [JsonProperty] private Color liveGraphPathogenDataColor = Color.FromArgb(255, 100, 150, 255);
        [JsonProperty] private Color liveGraphCureDataColor = Color.FromArgb(255, 255, 100, 255);
        
        [JsonProperty]
        private double liveGraphBackgroundOpacity = 0.6;

        [JsonProperty]
        private double liveGraphBackgroundDarkenOpacity = 0.0;
            
        [JsonProperty] private double liveGraphLineThickness = 2.0;
        [JsonProperty] private Color imageControlPointColor = Color.FromArgb(0xFF, 0xFF, 0x88, 0x00);
        [JsonProperty] private Color imageNormalPointColor = Color.FromArgb(0xFF, 0x00, 0xFF, 0x00);
        [JsonProperty] private Color imageHiddenPointColor = Color.FromArgb(0xFF, 0x00, 0xBD, 0xFF);
        [JsonProperty] private Color imageSelectedPointColor = Color.FromArgb(0xFF, 0xFF, 0x00, 0x00);
        [JsonProperty] private Color graphSubSelectionColor = Color.FromArgb(50, 200, 200, 0);
        [JsonProperty] private Color graphSelectionColor = Color.FromArgb(50, 255, 255, 0); 
        [JsonProperty] private Color graphCorridorColor = Color.FromArgb(100, 100, 200, 100);
        [JsonProperty] private Color graphResultDataRangeMarkerColor = Color.FromArgb(255, 250, 250, 250);
        [JsonProperty] private Color scaleRangeRangeColor_0_5 = Color.FromArgb(0xFF, 0x7C, 0xFC, 0x00);
        [JsonProperty] private Color scaleRangeRangeColor_5_10 = Color.FromArgb(255, 100, 200, 255);
        [JsonProperty] private Color scaleRangeRangeColor_10_20 = Color.FromArgb(255, 255, 185, 0);        
        [JsonProperty] private Color scaleRangeRangeColor_20_100 = Color.FromArgb(255, 255, 0, 0);

        [JsonProperty]
        private double graphResultDataRangeMarkerSize = 3.0;
        
        [JsonIgnore]
        public Color NormalDataColor { get { return this.normalDataColor; } set { this.normalDataColor = value; RaisePropertyChanged("NormalDataColor"); } }

        [JsonIgnore]
        public Color PathogenDataColor { get { return this.pathogenDataColor; } set { this.pathogenDataColor = value; RaisePropertyChanged("PathogenDataColor"); } }

        [JsonIgnore]
        public Color CureDataColor { get { return this.cureDataColor; } set { this.cureDataColor = value; RaisePropertyChanged("CureDataColor"); } }


        [JsonIgnore]
        public Color BaseDataColor { get { return this.baseDataColor; } set { this.baseDataColor = value; RaisePropertyChanged("BaseDataColor"); } }

        [JsonIgnore]
        public Color SelectedDataColor { get { return this.selectedDataColor; } set { this.selectedDataColor = value; RaisePropertyChanged("SelectedDataColor"); } }



        [JsonIgnore]
        public Color LiveGraphNormalDataColor { get { return this.liveGraphNormalDataColor; } set { this.liveGraphNormalDataColor = value; RaisePropertyChanged("LiveGraphNormalDataColor"); } }

        [JsonIgnore]
        public Color LiveGraphPathogenDataColor { get { return this.liveGraphPathogenDataColor; } set { this.liveGraphPathogenDataColor = value; RaisePropertyChanged("LiveGraphPathogenDataColor"); } }

        [JsonIgnore]
        public Color LiveGraphCureDataColor { get { return this.liveGraphCureDataColor; } set { this.liveGraphCureDataColor = value; RaisePropertyChanged("LiveGraphCureDataColor"); } }

        [JsonIgnore]
        public double LiveGraphBackgroundOpacity { get { return this.liveGraphBackgroundOpacity; } set { this.liveGraphBackgroundOpacity = value; RaisePropertyChanged("LiveGraphBackgroundOpacity"); } }

        [JsonIgnore]
        public double LiveGraphBackgroundDarkenOpacity { get { return this.liveGraphBackgroundDarkenOpacity; } set { this.liveGraphBackgroundDarkenOpacity = value; RaisePropertyChanged("LiveGraphBackgroundDarkenOpacity"); } }

        [JsonIgnore]
        public double LiveGraphLineThickness { get { return this.liveGraphLineThickness; } set { this.liveGraphLineThickness = value; RaisePropertyChanged("LiveGraphLineThickness"); } }

        [JsonIgnore]
        public Color ImageControlPointColor { get { return this.imageControlPointColor; } set { this.imageControlPointColor = value; RaisePropertyChanged("ImageControlPointColor"); } }

        [JsonIgnore]
        public Color ImageNormalPointColor { get { return this.imageNormalPointColor; } set { this.imageNormalPointColor = value; RaisePropertyChanged("ImageNormalPointColor"); } }

        [JsonIgnore]
        public Color ImageHiddenPointColor { get { return this.imageHiddenPointColor; } set { this.imageHiddenPointColor = value; RaisePropertyChanged("ImageHiddenPointColor"); } }

        [JsonIgnore]
        public Color ImageSelectedPointColor { get { return this.imageSelectedPointColor; } set { this.imageSelectedPointColor = value; RaisePropertyChanged("ImageSelectedPointColor"); RaisePropertyChanged("SelectedPointBrush"); } }

        [JsonIgnore]
        public Color GraphSubSelectionColor { get { return this.graphSubSelectionColor; } set { this.graphSubSelectionColor = value; RaisePropertyChanged("GraphSubSelectionColor"); } }

        [JsonIgnore]
        public Color GraphSelectionColor { get { return this.graphSelectionColor; } set { this.graphSelectionColor = value; RaisePropertyChanged("GraphSelectionColor"); } }

        [JsonIgnore]
        public Color GraphCorridorColor { get { return this.graphCorridorColor; } set { this.graphCorridorColor = value; RaisePropertyChanged("GraphCorridorColor"); } }

        [JsonIgnore]
        public Color GraphResultDataRangeMarkerColor { get { return this.graphResultDataRangeMarkerColor; } set { this.graphResultDataRangeMarkerColor = value; RaisePropertyChanged("GraphResultDataRangeMarkerColor"); } }

        [JsonIgnore]
        public double GraphResultDataRangeMarkerSize { get { return this.graphResultDataRangeMarkerSize; } set { this.graphResultDataRangeMarkerSize = value; RaisePropertyChanged("GraphResultDataRangeMarkerSize"); } }

        [JsonIgnore]
        public Color ScaleRangeRangeColor_0_5 { get { return this.scaleRangeRangeColor_0_5; } set { this.scaleRangeRangeColor_0_5 = value; RaisePropertyChanged("ScaleRangeRangeColor_0_5"); } }

        [JsonIgnore]
        public Color ScaleRangeRangeColor_5_10 { get { return this.scaleRangeRangeColor_5_10; } set { this.scaleRangeRangeColor_5_10 = value; RaisePropertyChanged("ScaleRangeRangeColor_5_10"); } }

        [JsonIgnore]
        public Color ScaleRangeRangeColor_10_20 { get { return this.scaleRangeRangeColor_10_20; } set { this.scaleRangeRangeColor_10_20 = value; RaisePropertyChanged("ScaleRangeRangeColor_10_20"); } }

        [JsonIgnore]
        public Color ScaleRangeRangeColor_20_100 { get { return this.scaleRangeRangeColor_20_100; } set { this.scaleRangeRangeColor_20_100 = value; RaisePropertyChanged("ScaleRangeRangeColor_20_100"); } }

        [JsonIgnore]
        public Brush SelectedPointBrush { get { return new SolidColorBrush(this.ImageSelectedPointColor); } private set { } }
        

        /// <summary>
        /// 
        /// </summary>
        static UiConfig()
        {
            string filename = AppConfig.PALETTE_CONFIG_FILENAME;

            if (!File.Exists(filename))
            {
                UiConfig.instance = new UiConfig();                

                try
                {
                    string json = JsonConvert.SerializeObject(UiConfig.instance, Formatting.Indented);
                    File.WriteAllText(filename, json);
                }
                catch (Exception)
                { }
            }
            else
            {
                UiConfig.instance = JsonConvert.DeserializeObject<UiConfig>(File.ReadAllText(filename));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private UiConfig()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        public void Revert()
        {
            string filename = AppConfig.PALETTE_CONFIG_FILENAME;

            try
            {
                UiConfig inst = JsonConvert.DeserializeObject<UiConfig>(File.ReadAllText(filename));

                foreach (var prop in typeof(UiConfig).GetProperties())
                {
                    if (typeof(UiConfig) == prop.PropertyType)
                        continue;

                    prop.SetValue(this, prop.GetValue(inst));
                }
            }
            catch (Exception e)
            {
                Console.Out.Write(e.StackTrace);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void Default()
        {
            string filename = AppConfig.PALETTE_CONFIG_FILENAME;

            try
            {
                UiConfig inst = new UiConfig();

                foreach (var prop in typeof(UiConfig).GetProperties())
                {
                    if (typeof(UiConfig) == prop.PropertyType)
                        continue;

                    prop.SetValue(this, prop.GetValue(inst));
                }
            }
            catch (Exception e)
            {
                Console.Out.Write(e.StackTrace);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void Save()
        {
            string filename = AppConfig.PALETTE_CONFIG_FILENAME;

            try
            {
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(filename, json);
            }
            catch (Exception e)
            {
                Console.Out.Write(e.StackTrace);
            }
        }
    }
}
