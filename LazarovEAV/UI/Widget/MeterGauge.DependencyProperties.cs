using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace LazarovEAV.UI
{
    /// <summary>
    /// 
    /// </summary>
    partial class MeterGauge
    {
        public static readonly DependencyProperty DisplayedValueProperty =
                                                      DependencyProperty.Register("DisplayedValue", typeof(double), typeof(MeterGauge),
                                                      new PropertyMetadata(0.0, (o, arg) => { ((MeterGauge)o).renderPointer(); }));

        public static readonly DependencyProperty BackgroundBrushProperty =
                                                      DependencyProperty.Register("BackgroundBrush", typeof(Brush), typeof(MeterGauge),
                                                      new PropertyMetadata(null, (o, arg) => { ((MeterGauge)o).renderBackground(); }));

        public static readonly DependencyProperty BorderBrushProperty =
                                                      DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(MeterGauge),
                                                      new PropertyMetadata(Brushes.Gray, (o, arg) => { ((MeterGauge)o).renderBorder(); }));

        public static readonly DependencyProperty BorderPenProperty =
                                                  DependencyProperty.Register("BorderPen", typeof(Brush), typeof(MeterGauge),
                                                  new PropertyMetadata(Brushes.Gray, (o, arg) => { ((MeterGauge)o).renderBorder(); }));

        public static readonly DependencyProperty RenderHeightProperty =
                                                  DependencyProperty.Register("RenderHeight", typeof(double), typeof(MeterGauge),
                                                  new PropertyMetadata(10.0, null));

        public static readonly DependencyProperty RenderWidthProperty =
                                          DependencyProperty.Register("RenderWidth", typeof(double), typeof(MeterGauge),
                                          new PropertyMetadata(10.0, null));

        public static readonly DependencyProperty TitleTextProperty =
                                                  DependencyProperty.Register("TitleText", typeof(string), typeof(MeterGauge),
                                                  new PropertyMetadata("", (o, arg) => { ((MeterGauge)o).renderTitle(); }));

        public static readonly DependencyProperty RangeStartProperty =
                                                  DependencyProperty.Register("RangeStart", typeof(double), typeof(MeterGauge),
                                                  new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, (o, arg) => { ((MeterGauge)o).renderRange(); }));

        public static readonly DependencyProperty RangeEndProperty =
                                                  DependencyProperty.Register("RangeEnd", typeof(double), typeof(MeterGauge),
                                                  new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, (o, arg) => { ((MeterGauge)o).renderRange(); }));

        public static readonly DependencyProperty RangeBrushProperty =
                                                      DependencyProperty.Register("RangeBrush", typeof(Brush), typeof(MeterGauge),
                                                      new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, (o, arg) => { ((MeterGauge)o).renderRange(); }));

        public double DisplayedValue
        {
            get { return (double)GetValue(DisplayedValueProperty); }
            set { SetValue(DisplayedValueProperty, value); }
        }

        public Brush BackgroundBrush
        {
            get { return (Brush)GetValue(BackgroundBrushProperty); }
            set { SetValue(BackgroundBrushProperty, value); }
        }

        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public Brush BorderPen
        {
            get { return (Brush)GetValue(BorderPenProperty); }
            set { SetValue(BorderPenProperty, value); }
        }

        public double RenderHeight
        {
            get { return (double)GetValue(RenderHeightProperty); }
            protected set { SetValue(RenderHeightProperty, value); }
        }

        public double RenderWidth
        {
            get { return (double)GetValue(RenderWidthProperty); }
            protected set { SetValue(RenderWidthProperty, value); }
        }

        public string TitleText
        {
            get { return (string)GetValue(TitleTextProperty); }
            set { SetValue(TitleTextProperty, value); }
        }

        public double RangeStart
        {
            get { return (double)GetValue(RangeStartProperty); }
            set { SetValue(RangeStartProperty, value); }
        }

        public double RangeEnd
        {
            get { return (double)GetValue(RangeEndProperty); }
            set { SetValue(RangeEndProperty, value); }
        }

        public Brush RangeBrush
        {
            get { return (Brush)GetValue(RangeBrushProperty); }
            set { SetValue(RangeBrushProperty, value); }
        }
    }
}
