using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LazarovEAV.UI
{
    /// <summary>
    /// Interaction logic for UiSettingsDialog.xaml
    /// </summary>
    public partial class UiSettingsDialog : Window
    {
        private class ComboItem
        {
            public string Label { get; set; }
            public string Property { get; set; }
        }

        private ComboItem[] items = 
        {
            new ComboItem(){ Label = "Нормално измерване", Property = "NormalDataColor"},
            new ComboItem(){ Label = "Измерване с патоген", Property = "PathogenDataColor"},
            new ComboItem(){ Label = "Измерване с лекарство", Property = "CureDataColor"},
            new ComboItem(){ Label = "Графика нормално измерване", Property = "LiveGraphNormalDataColor"},
            new ComboItem(){ Label = "Графика измерване с патоген", Property = "LiveGraphPathogenDataColor"},
            new ComboItem(){ Label = "Графика измерване с лекарство", Property = "LiveGraphCureDataColor"},
            new ComboItem(){ Label = "Контролна точка", Property = "ImageControlPointColor"},
            new ComboItem(){ Label = "Нормална точка", Property = "ImageNormalPointColor"},
            new ComboItem(){ Label = "Скрита точка", Property = "ImageHiddenPointColor"},
            new ComboItem(){ Label = "Точка на измерване", Property = "ImageSelectedPointColor"},
            new ComboItem(){ Label = "Селекция тип измерване в графиката", Property = "GraphSubSelectionColor"},
            new ComboItem(){ Label = "Селекция на точка в графиката", Property = "GraphSelectionColor"},
            new ComboItem(){ Label = "Физиологичен коридор", Property = "GraphCorridorColor"},
            new ComboItem(){ Label = "Маркер за минимум и максимум в графиката", Property = "GraphResultDataRangeMarkerColor"},
            new ComboItem(){ Label = "Пад на стелката от 0 до 5 единици", Property = "ScaleRangeRangeColor_0_5"},
            new ComboItem(){ Label = "Пад на стелката от 5 до 10 единици", Property = "ScaleRangeRangeColor_5_10"},
            new ComboItem(){ Label = "Пад на стелката от 10 до 20 единици", Property = "ScaleRangeRangeColor_10_20"},
            new ComboItem(){ Label = "Пад на стелката от 20 до 100 единици", Property = "ScaleRangeRangeColor_20_100"},
        };


        /// <summary>
        /// 
        /// </summary>
        public UiSettingsDialog()
        {
            InitializeComponent();

            for (int i = 0; i < this.items.Length; i++)
            {
                this.cbElements.Items.Add(this.items[i].Label);
            }

            this.cbElements.SelectedIndex = 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext.GetType().GetMethod("Save").Invoke(this.DataContext, null);
            this.Close();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DataContext.GetType().GetMethod("Revert").Invoke(this.DataContext, null);
            this.Close();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbElements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateColorBars();
        }


        /// <summary>
        /// 
        /// </summary>
        private void updateColorBars()
        {
            int idx = this.cbElements.SelectedIndex;

            if (idx >= 0 && idx < this.items.Length)
            {
                Color clr = (Color)this.DataContext.GetType().GetProperty(this.items[idx].Property).GetValue(this.DataContext, null);

                this.transpSlider.Value = clr.A;
                this.redSlider.Value = clr.R;
                this.greenSlider.Value = clr.G;
                this.blueSlider.Value = clr.B;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int idx = this.cbElements.SelectedIndex;

            if (idx >= 0 && idx < this.items.Length)
            {
                Color clr = Color.FromArgb((byte)this.transpSlider.Value, (byte)this.redSlider.Value, (byte)this.greenSlider.Value, (byte)this.blueSlider.Value);
                this.DataContext.GetType().GetProperty(this.items[idx].Property).SetValue(this.DataContext, clr);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.DataContext.GetType().GetProperty("LiveGraphBackgroundOpacity").SetValue(this.DataContext, e.NewValue);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slider_ValueChanged_2(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.DataContext.GetType().GetProperty("LiveGraphBackgroundDarkenOpacity").SetValue(this.DataContext, e.NewValue);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slider_ValueChanged_3(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.DataContext.GetType().GetProperty("LiveGraphLineThickness").SetValue(this.DataContext, e.NewValue);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.DataContext.GetType().GetMethod("Default").Invoke(this.DataContext, null);
            updateColorBars();
        }
    }
}
