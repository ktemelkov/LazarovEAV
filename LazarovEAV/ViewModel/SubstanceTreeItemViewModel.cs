using LazarovEAV.Model;
using LazarovEAV.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    class SubstanceTreeItemViewModel : NotifyPropertyChangedImpl
    {
        private object internalItem;
        internal object InternalItem { get { return this.internalItem; } }


        /// <summary>
        /// 
        /// </summary>
        public long Id
        {
            get
            {
                if (this.Substance != null)
                    return this.Substance.Id;

                if (this.Folder != null)
                    return this.Folder.Id;

                return 0;
            }

            set
            {
                if (this.Substance != null)
                    this.Substance.Id = value;
                else if (this.Folder != null)
                    this.Folder.Id = value;

                RaisePropertyChanged("Id");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string Name {
            get {
                if (this.Substance != null)
                    return this.Substance.Name;

                if (this.Folder != null)
                    return this.Folder.Name;

                return null;
            }

            set {
                if (this.Substance != null)
                    this.Substance.Name = value;
                else if (this.Folder != null)
                    this.Folder.Name = value;

                RaisePropertyChanged("Name");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string Description {
            get {
                if (this.Substance != null)
                    return this.Substance.Description;

                return null;
            }

            set {
                if (this.Substance != null)
                    this.Substance.Description = value;

                RaisePropertyChanged("Description");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public SubstanceType Type {
            get {
                if (this.Substance != null)
                    return this.Substance.Type;

                return SubstanceType.OTHER;
            }

            set {
                if (this.Substance != null)
                    this.Substance.Type = value;

                RaisePropertyChanged("Type");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public long Parent_Id
        {
            get
            {
                if (this.Substance != null)
                    return this.Substance.Folder_Id;

                if (this.Folder != null)
                    return this.Folder.Parent_Id;

                return 0;
            }

            set
            {
                if (this.Substance != null)
                    this.Substance.Folder_Id = value;
                else if (this.Folder != null)
                    this.Folder.Parent_Id = value;

                RaisePropertyChanged("Parent_Id");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public SubstanceInfo Substance
        {
            get {
                if (this.internalItem == null || !(this.internalItem is SubstanceInfo))
                    return null;

                return (SubstanceInfo)this.internalItem;
            }

            set {
                this.internalItem = value;
                RaisePropertyChanged("Substance");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public SubstanceFolder Folder
        {
            get
            {
                if (this.internalItem == null || !(this.internalItem is SubstanceFolder))
                    return null;

                return (SubstanceFolder)this.internalItem;
            }

            set
            {
                this.internalItem = value;
                RaisePropertyChanged("Folder");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public SubstanceTreeItemViewModel(object item) 
        {
            this.internalItem = item;
        }


        /// <summary>
        /// 
        /// </summary>
        public SubstanceTreeItemViewModel() 
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pivm"></param>
        public void CopyTo(SubstanceTreeItemViewModel vm)
        {
            if (this.internalItem == null || vm.internalItem == null)
                return;

            vm.Id = this.Id;
            vm.Parent_Id = this.Parent_Id;
            vm.Name = this.Name;
            vm.Type = this.Type;
            vm.Description = this.Description;
        }
    }
}
