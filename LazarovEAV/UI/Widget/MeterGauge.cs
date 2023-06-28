using LazarovEAV.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    partial class MeterGauge : FrameworkElement
    {
        private VisualCollection children;
        private DrawingVisual background = new DrawingVisual();
        private DrawingVisual title = new DrawingVisual();
        private DrawingVisual range = new DrawingVisual();
        private DrawingVisual scale = new DrawingVisual();
        private DrawingVisual pointer = new DrawingVisual();
        private DrawingVisual border = new DrawingVisual();


        /// <summary>
        /// 
        /// </summary>
        public MeterGauge()
        {
            this.children = new VisualCollection(this);
            this.children.Add(this.background);
            this.children.Add(this.title);
            this.children.Add(this.range);
            this.children.Add(this.scale);
            this.children.Add(this.pointer);
            this.children.Add(this.border);
            
            this.SizeChanged += (s, e) => 
            {
                MeterGaugeMetrics metrics = new MeterGaugeMetrics(this.ActualWidth, this.ActualHeight);
                this.RenderTransform = new TranslateTransform((this.ActualWidth - metrics.CX) / 2, (this.ActualHeight - metrics.CY) / 2);
                this.Clip = new RectangleGeometry(new Rect(new Point(0, 0), new Point(metrics.CX, metrics.CY)));
                this.RenderHeight = metrics.CY + 1;
                this.RenderWidth = metrics.CX + 1;

                renderAll();
            };
        }


        /// <summary>
        /// 
        /// </summary>
        private void renderAll()
        {
            renderBackground();
            renderTitle();
            renderRange();
            renderScale();
            renderPointer();
            renderBorder();
        }


        /// <summary>
        /// 
        /// </summary>
        private void renderBorder()
        {
            MeterGaugeMetrics m = new MeterGaugeMetrics(this.ActualWidth, this.ActualHeight);

            double startAngle = 0;
            double endAngle = Math.PI;
            double r = m.OuterRadius/3.4;
            Point c0 = m.Center;

            Point arcPt1 = new Point(c0.X + Math.Cos(startAngle) * r, c0.Y - Math.Sin(startAngle) * r);
            Point arcPt2 = new Point(c0.X + Math.Cos(endAngle) * r, c0.Y - Math.Sin(endAngle) * r);

            List<PathFigure> figures = new List<PathFigure>(1) 
            { 
                new PathFigure(arcPt1, new List<PathSegment>() 
                { 
                    new ArcSegment(arcPt2, new Size(r, r), 0.0, false, SweepDirection.Counterclockwise, true) 
                }, true)
            };

            figures[0].IsClosed = true;

            Geometry g = new PathGeometry(figures, FillRule.EvenOdd, null);

            
            DrawingVisual v = new DrawingVisual();

            using (DrawingContext dc = v.RenderOpen())
            {
                Pen borderPen = new Pen(this.BorderPen, 1);                
                double offset = 1.5;

                dc.DrawLine(borderPen, new Point(m.CX / 70 + offset, m.CY / 30), new Point(m.CX / 70 + offset, m.CY - m.CY / 10));
                dc.DrawLine(borderPen, new Point(m.CX / 70, m.CY / 30 + offset), new Point(m.CX - m.CX / 70, m.CY / 30 + offset));
                dc.DrawLine(borderPen, new Point(m.CX / 70, m.CY - m.CY / 10 - offset/8), new Point(m.CX - m.CX / 70, m.CY - m.CY / 10 - offset/8));
                dc.DrawLine(borderPen, new Point(m.CX - m.CX / 70 - offset, m.CY / 30), new Point(m.CX - m.CX / 70 - offset, m.CY - m.CY / 10));

                dc.DrawGeometry(this.BorderBrush, borderPen, g);

                dc.DrawRectangle(this.BorderBrush, null, new Rect(new Point(1, m.CY * 9 / 10), new Size(m.CX - 2, m.CY / 10)));
                dc.DrawRectangle(this.BorderBrush, null, new Rect(new Point(1, 1), new Size(m.CX - 2, m.CY / 30)));
                dc.DrawRectangle(this.BorderBrush, null, new Rect(new Point(1, 1), new Size(m.CX / 70, m.CY - m.CY / 10)));
                dc.DrawRectangle(this.BorderBrush, null, new Rect(new Point(m.CX - m.CX / 70 - 1, 1), new Size(m.CX / 70, m.CY - m.CY / 10)));


                dc.DrawEllipse(this.BorderPen, null, new Point(m.CX / 2, m.CY * 9 / 10 + m.CY / 120), m.CX / 30, m.CX / 30);

                dc.DrawRectangle(this.BorderBrush, null, new Rect(new Point(m.CX / 2 - m.CX / 34, m.CY * 9 / 10),
                                    new Size(m.CX / 17, m.CY / 60)));

                dc.DrawRectangle(null, borderPen, new Rect(new Point(1, 1), new Size(m.CX - 2, m.CY - 2)));
            }

            this.border.Children.Clear();
            this.border.Children.Add(v);
        }


        /// <summary>
        /// 
        /// </summary>
        private void renderPointer()
        {
            MeterGaugeMetrics m = new MeterGaugeMetrics(this.ActualWidth, this.ActualHeight);
            
            Point pt0 = new Point(m.Center.X - m.OuterRadius / 4, m.Center.Y);
            Point pt1 = new Point(m.Center.X - m.OuterRadius - m.MajorTickSize, m.Center.Y);
            Point pt2 = new Point(m.Center.X, m.Center.Y);
            Point pt3 = new Point(m.Center.X - m.OuterRadius/2, m.Center.Y);

            DrawingVisual v = new DrawingVisual();

            using (DrawingContext dc = v.RenderOpen())
            {
                dc.DrawLine(new Pen(Brushes.Red, 1), pt0, pt1);
                dc.DrawLine(new Pen(Brushes.WhiteSmoke, 3), pt2, pt3);
            }

            double angle = MeterGaugeMetrics.ANGLE0 + this.DisplayedValue * (MeterGaugeMetrics.ANGLE100 - MeterGaugeMetrics.ANGLE0) / 100;
            v.Transform = new RotateTransform(angle, m.Center.X, m.Center.Y);

            this.pointer.Children.Clear();
            this.pointer.Children.Add(v);
        }


        /// <summary>
        /// 
        /// </summary>
        private void renderScale()
        {
            MeterGaugeMetrics metrics = new MeterGaugeMetrics(this.ActualWidth, this.ActualHeight);

            this.scale.Children.Clear();

            renderScaleBase(metrics);
            renderScaleTicks(metrics);
            renderScaleLabels(metrics);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        private void renderScaleLabels(MeterGaugeMetrics metrics)
        {
            DrawingVisual vLabelsMj = createTickLabels(metrics.Center, metrics.OuterRadius + metrics.MajorTickSize, 
                                                        MeterGaugeMetrics.ANGLE0, MeterGaugeMetrics.ANGLE100,
                                                        metrics.MajorFontSize, Brushes.Black,
                                                        0, 100, 10, -1);

            DrawingVisual vLabelsMn = createTickLabels(metrics.Center, metrics.InnerRadius - metrics.MinorTickSize,
                                                        MeterGaugeMetrics.ANGLE0, MeterGaugeMetrics.ANGLE100,
                                                        metrics.MinorFontSize, Brushes.Black,
                                                        5, 95, 10, 0);

            DrawingVisual vLabelsWin = createTickLabels(metrics.Center, metrics.WindowRadius - metrics.MinorTickSize,
                                                        MeterGaugeMetrics.ANGLE0, MeterGaugeMetrics.ANGLE100,
                                                        metrics.MinorFontSize, Brushes.Black,
                                                        AppConfig.EAV_WINDOW_BEGIN, AppConfig.EAV_WINDOW_END, 
                                                        AppConfig.EAV_WINDOW_END - AppConfig.EAV_WINDOW_BEGIN, 0);

            this.scale.Children.Add(vLabelsMj);
            this.scale.Children.Add(vLabelsMn);
            this.scale.Children.Add(vLabelsWin);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        private void renderScaleTicks(MeterGaugeMetrics metrics)
        {
            DrawingVisual vTicks1 = createTicks(metrics.Center, metrics.OuterRadius, MeterGaugeMetrics.ANGLE0, MeterGaugeMetrics.ANGLE100, metrics.MajorTickSize, 0, 100, 10);
            DrawingVisual vTicks2 = createTicks(metrics.Center, metrics.OuterRadius, MeterGaugeMetrics.ANGLE0, MeterGaugeMetrics.ANGLE100, metrics.MinorTickSize, 5, 95, 10);

            DrawingVisual vTicksMn = createTicks(metrics.Center, metrics.InnerRadius, MeterGaugeMetrics.ANGLE0, MeterGaugeMetrics.ANGLE100, -metrics.MinorTickSize, 5, 95, 10);

            DrawingVisual vTicksW = createTicks(metrics.Center, metrics.WindowRadius, MeterGaugeMetrics.ANGLE0, MeterGaugeMetrics.ANGLE100, -metrics.MinorTickSize, 
                                                    AppConfig.EAV_WINDOW_BEGIN, AppConfig.EAV_WINDOW_END, AppConfig.EAV_WINDOW_END - AppConfig.EAV_WINDOW_BEGIN);
            DrawingVisual vTicksW2 = createTicks(metrics.Center, metrics.WindowRadius, MeterGaugeMetrics.ANGLE0, MeterGaugeMetrics.ANGLE100, -metrics.MinorTickSize / 2, 
                                                    AppConfig.EAV_WINDOW_BEGIN, AppConfig.EAV_WINDOW_END, 3);

            this.scale.Children.Add(vTicks1);
            this.scale.Children.Add(vTicks2);
            this.scale.Children.Add(vTicksMn);
            this.scale.Children.Add(vTicksW);
            this.scale.Children.Add(vTicksW2);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        private void renderScaleBase(MeterGaugeMetrics metrics)
        {
            DrawingVisual v = new DrawingVisual();

            using (DrawingContext dc = v.RenderOpen())
            {
                dc.DrawGeometry(null, new Pen(Brushes.LightGreen, metrics.MinorTickSize), createWindowMark(metrics));
                dc.DrawGeometry(null, new Pen(Brushes.Black, 2), createOuterScale(metrics));
                dc.DrawGeometry(null, new Pen(Brushes.Black, 1), createInnerScale(metrics));
                dc.DrawGeometry(null, new Pen(Brushes.Black, 1), createWindowScale(metrics));
            }

            this.scale.Children.Add(v);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        /// <returns></returns>
        private Geometry createWindowScale(MeterGaugeMetrics metrics)
        {
            double step = (MeterGaugeMetrics.ANGLE100 - MeterGaugeMetrics.ANGLE0) / 100;
            double begin = MeterGaugeMetrics.ANGLE0 + (100 - AppConfig.EAV_WINDOW_END) * step;
            double end = MeterGaugeMetrics.ANGLE0 + (100 - AppConfig.EAV_WINDOW_BEGIN) * step;

            return createScale(metrics.Center, metrics.WindowRadius, begin, end);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        /// <returns></returns>
        private Geometry createInnerScale(MeterGaugeMetrics metrics)
        {
            double offset = 5 * (MeterGaugeMetrics.ANGLE100 - MeterGaugeMetrics.ANGLE0) / 100;
            double begin = MeterGaugeMetrics.ANGLE0 + offset;
            double end = MeterGaugeMetrics.ANGLE100 - offset;

            return createScale(metrics.Center, metrics.InnerRadius, begin, end);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        /// <returns></returns>
        private Geometry createOuterScale(MeterGaugeMetrics metrics)
        {
            return createScale(metrics.Center, metrics.OuterRadius, MeterGaugeMetrics.ANGLE0, MeterGaugeMetrics.ANGLE100);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="metrics"></param>
        /// <returns></returns>
        private Geometry createWindowMark(MeterGaugeMetrics metrics)
        {
            double step = (MeterGaugeMetrics.ANGLE100 - MeterGaugeMetrics.ANGLE0) / 100;
            double winStart = MeterGaugeMetrics.ANGLE0 + (100 - AppConfig.EAV_WINDOW_END) * step;
            double winEnd = MeterGaugeMetrics.ANGLE0 + (100 - AppConfig.EAV_WINDOW_BEGIN) * step;

            return createScale(metrics.Center, metrics.OuterRadius + metrics.MinorTickSize/2, winStart, winEnd);
        }


        /// <summary>
        /// 
        /// </summary>
        private void renderBackground()
        {
            this.background.Children.Clear();

            MeterGaugeMetrics metrics = new MeterGaugeMetrics(this.ActualWidth, this.ActualHeight);
            DrawingVisual v = new DrawingVisual();
          
            using (DrawingContext dc = v.RenderOpen())
            {
                dc.DrawRectangle(this.BackgroundBrush, null, new Rect(0, 0, metrics.CX, metrics.CY));                
            }

            this.background.Children.Add(v);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="c0"></param>
        /// <param name="r"></param>
        /// <param name="startAngle"></param>
        /// <param name="endAngle"></param>
        /// <param name="majorSize"></param>
        /// <param name="minorSize"></param>
        /// <returns></returns>
        private DrawingVisual createTicks(Point c0, double r, double startAngle, double endAngle, double tickSize, double start, double stop, double step)
        {
            DrawingVisual v = new DrawingVisual();
          
            double stepAngle = (endAngle - startAngle) / 100.0;

            using (DrawingContext dc = v.RenderOpen())
            {
                for (double i = start; i <= stop; i += step)
                {
                    dc.PushTransform(new RotateTransform(startAngle + i * stepAngle - 90, c0.X, c0.Y));

                    dc.DrawLine(new Pen(Brushes.Black, 1), new Point(c0.X, c0.Y - r), new Point(c0.X, c0.Y - r - tickSize));

                    dc.Pop();
                }
            }

            return v;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="c0"></param>
        /// <param name="r"></param>
        /// <param name="startAngle"></param>
        /// <param name="endAngle"></param>
        /// <param name="fontSize"></param>
        /// <param name="fontBrush"></param>
        /// <param name="majorSize"></param>
        /// <param name="minorSize"></param>
        /// <param name="offs"></param>
        /// <returns></returns>
        private DrawingVisual createTickLabels(Point c0, double r, double startAngle, double endAngle, double fontSize, Brush fontBrush, double start, double end, double step, double offs)
        {
            DrawingVisual v = new DrawingVisual();

            double stepAngle = (endAngle - startAngle) / 100.0;

            using (DrawingContext dc = v.RenderOpen())
            {
                for (double i = start; i <= end; i += step)
                {
                    FormattedText ft = new FormattedText(String.Format("{0:D}", (int)i), new CultureInfo(1033), new FlowDirection(), new Typeface("Ariel"), fontSize, fontBrush);

                    dc.PushTransform(new RotateTransform(startAngle + i * stepAngle - 90, c0.X, c0.Y));

                    dc.DrawText(ft, new Point(c0.X - ft.Width/2, c0.Y - r + offs*(ft.Baseline+2)));
                    dc.Pop();
                }
            }

            return v;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="c0"></param>
        /// <param name="r"></param>
        /// <param name="startAngle"></param>
        /// <param name="endAngle"></param>
        /// <returns></returns>
        private Geometry createScale(Point c0, double r, double startAngle, double endAngle)
        {
            startAngle = startAngle * Math.PI / 180.0;
            endAngle = endAngle * Math.PI / 180.0;

            Point arcPt1 = new Point(c0.X + Math.Cos(startAngle) * r, c0.Y - Math.Sin(startAngle) * r);
            Point arcPt2 = new Point(c0.X + Math.Cos(endAngle) * r, c0.Y - Math.Sin(endAngle) * r);

            List<PathFigure> figures = new List<PathFigure>(1) 
            { 
                new PathFigure(arcPt1, new List<PathSegment>() 
                { 
                    new ArcSegment(arcPt2, new Size(r, r), 0.0, false, SweepDirection.Counterclockwise, true) 
                }, true)
            };

            figures[0].IsClosed = false;

            return new PathGeometry(figures, FillRule.EvenOdd, null);
        }


        /// <summary>
        /// 
        /// </summary>
        protected override int VisualChildrenCount 
        {
            get { return this.children.Count; }
        }
 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= this.children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return this.children[index];
        }


        /// <summary>
        /// 
        /// </summary>
        private void renderTitle()
        {
            MeterGaugeMetrics metrics = new MeterGaugeMetrics(this.ActualWidth, this.ActualHeight);

            DrawingVisual v = new DrawingVisual();

            using (DrawingContext dc = v.RenderOpen())
            {
                FormattedText ft = new FormattedText(this.TitleText, new CultureInfo(1033), new FlowDirection(), new Typeface("Ariel"), metrics.TitleFontSize, Brushes.Black);

                dc.DrawText(ft, new Point(metrics.TitleX - ft.Width / 2, metrics.TitleY - ft.Height / 2));
            }

            this.title.Children.Clear();
            this.title.Children.Add(v);
        }


        /// <summary>
        /// 
        /// </summary>
        private void renderRange()
        {
            MeterGaugeMetrics metrics = new MeterGaugeMetrics(this.ActualWidth, this.ActualHeight);

            double rangeBegin = this.RangeStart;
            double rangeEnd = this.RangeEnd;

            if (rangeBegin > rangeEnd)
            {
                rangeBegin = this.RangeEnd;
                rangeEnd = this.RangeStart;
            }

            double step = (MeterGaugeMetrics.ANGLE100 - MeterGaugeMetrics.ANGLE0) / 100;
            double begin = MeterGaugeMetrics.ANGLE0 + (100 - rangeEnd) * step;
            double end = MeterGaugeMetrics.ANGLE0 + (100 - rangeBegin) * step;
            double thickness = metrics.OuterRadius - metrics.InnerRadius;
            double r = metrics.InnerRadius + thickness/2.0;

            DrawingVisual v = new DrawingVisual();

            using (DrawingContext dc = v.RenderOpen())
            {
                dc.DrawGeometry(null, new Pen(this.RangeBrush, thickness), createScale(metrics.Center, r, begin, end));
            }

            this.range.Children.Clear();
            this.range.Children.Add(v);
        }
    }
}
