using LazarovEAV.Config;
using LazarovEAV.Model;
using LazarovEAV.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LazarovEAV.UI
{
    /// <summary>
    /// Interaction logic for HandImagePanel.xaml
    /// </summary>
    public partial class HandImagePanel : UserControl
    {
        public static readonly DependencyProperty BodySideProperty =
                        DependencyProperty.Register("BodySide", typeof(PositionType), typeof(HandImagePanel),
                        new FrameworkPropertyMetadata(PositionType.RIGHT));

        internal PositionType BodySide { get { return (PositionType)GetValue(BodySideProperty); } set { SetValue(BodySideProperty, value); } }


        public static readonly DependencyProperty InactiveOpacityProperty =
                        DependencyProperty.Register("InactiveOpacity", typeof(double), typeof(HandImagePanel),
                        new FrameworkPropertyMetadata(0.5));

        internal double InactiveOpacity { get { return (double)GetValue(InactiveOpacityProperty); } set { SetValue(InactiveOpacityProperty, value); } }


        private static string[] originalImageSources = new string[] 
        { 
            "pack://application:,,,/LazarovEAV;component/Resources/hand_dorsal.png",
            "pack://application:,,,/LazarovEAV;component/Resources/hand_palmar.png",
            "pack://application:,,,/LazarovEAV;component/Resources/foot_top.png",
            "pack://application:,,,/LazarovEAV;component/Resources/foot_inside.png",
            "pack://application:,,,/LazarovEAV;component/Resources/foot_outside.png",
        };

        public static string[] imageSources = { };


        /// <summary>
        /// 
        /// </summary>
        static HandImagePanel()
        {
            string filename = AppConfig.SUBST_IMAGE_CONFIG_FILENAME;

            if (!File.Exists(filename))
            {
                string json = JsonConvert.SerializeObject(originalImageSources, Formatting.Indented);
                File.WriteAllText(filename, json);
            }

            HandImagePanel.imageSources = JsonConvert.DeserializeObject<string[]>(File.ReadAllText(filename));

            if (HandImagePanel.imageSources.Length != HandImagePanel.originalImageSources.Length)
            {
                HandImagePanel.imageSources = (string[])HandImagePanel.originalImageSources.Clone();

                string json = JsonConvert.SerializeObject(HandImagePanel.imageSources);
                File.WriteAllText(filename, json);
            }

            for (int i = 0; i < HandImagePanel.imageSources.Length; i++)
            {
                if (!File.Exists(HandImagePanel.imageSources[i]))
                    HandImagePanel.imageSources[i] = HandImagePanel.originalImageSources[i];
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public HandImagePanel()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pointEllipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var o = (DependencyObject)sender;

            object m = o.GetValue(ImagePanelProperties.ActualMeridianProperty);
            object p = o.GetValue(ImagePanelProperties.ActualPointProperty);

            if (m != null)
            {
                PropertyInfo propertyInfo = m.GetType().GetProperty("SelectedPoint", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod);

                if (propertyInfo != null)
                    propertyInfo.SetValue(m, p);
            }
        }
    }
}
