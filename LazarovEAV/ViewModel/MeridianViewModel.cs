using LazarovEAV.Model;
using LazarovEAV.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class MeridianViewModel : NotifyPropertyChangedImpl
    {
        private MeridianInfo meridian;
        private MeridianPointViewModel selectedPoint;
        private List<MeridianPointViewModel> points;

        public MeridianInfo Model { get { return this.meridian; } private set { } }

        public long Id { get { return this.meridian.Id; } private set { } }
        public string Name { get { return this.meridian.Name; } }
        public string Description { get { return this.meridian.Description;  } }

        public List<MeridianPointViewModel> Points { get { return this.points; } }
        public MeridianPointViewModel SelectedPoint { get { return this.selectedPoint; } set { RaisePropertyChanged("SelectedPoint", this.selectedPoint, this.selectedPoint = value); RaisePropertyChanged("SelectedPointIndex"); } }

        public int SelectedPointIndex 
        { 
            get {
                if (this.SelectedPoint == null || this.Points == null)
                    return -1;

                return this.Points.IndexOf(this.SelectedPoint); 
            } 

            set {
                if (this.Points == null || value < 0 || value >= this.Points.Count)
                    this.SelectedPoint = null;
                else
                    this.SelectedPoint = this.Points[value];
            } 
        }


        /// <summary>
        /// 
        /// </summary>
        public MeridianViewModel(MeridianInfo info)
        {
            this.meridian = info;

            if (!DesignerProperties.GetIsInDesignMode(new Mock.MockDependencyObject()))
            {            
                if (this.meridian != null && this.meridian.Points != null)
                {
                    this.points = (from p in this.meridian.Points.OrderBy(pi => pi.SortKey) select new MeridianPointViewModel(p, this)).ToList<MeridianPointViewModel>();

                    if (this.points.Count() > 0)
                        this.selectedPoint = this.points.ElementAt(0);
                }
            }
            else
            {
                this.points = new List<MeridianPointViewModel>()
                {
                    new MeridianPointViewModel(new MeridianPoint(){ Name="P1", DescriptionLeft="[b]Test Meridian 1[/b] Point something and some [i]long l[l]ong long[/l] text follows[/i] here for [l]testing the html control blah[/l] blah blahblah blah blah blah blah blah blahblah  blahblahblah", X = 100, Y = 100 }, this),
                    new MeridianPointViewModel(new MeridianPoint(){ Name="P2", DescriptionLeft="Test Point 2", X = 150, Y = 150  }, this),
                    new MeridianPointViewModel(new MeridianPoint(){ Name="P3", DescriptionLeft="Test Point 3", X = 150, Y = 200  }, this),
                    new MeridianPointViewModel(new MeridianPoint(){ Name="P4", DescriptionLeft="Test Point 4", X = 200, Y = 200, ImageIndex = 1 }, this),
                };

                this.selectedPoint = this.points.ElementAt(0);
            }
        }
    }
}
