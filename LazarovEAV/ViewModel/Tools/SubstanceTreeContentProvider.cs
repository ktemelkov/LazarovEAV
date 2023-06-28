using Aga.Controls.Tree;
using LazarovEAV.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazarovEAV.ViewModel
{
    class SubstanceTreeContentProvider : ITreeModel
    {
        private EntityManager database;
        private string filter;
        private List<SubstanceTreeItemViewModel> preloaded = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        public SubstanceTreeContentProvider(EntityManager db, string filter = null)
        {
            this.database = db;
            this.filter = !string.IsNullOrEmpty(filter) ? filter.ToLower(): null;

            if (this.filter == null)
            {
                this.preloaded = this.database.loadItems<SubstanceFolder>().Select(sf => new SubstanceTreeItemViewModel(sf))
                        .Concat(this.database.loadItems<SubstanceInfo>().Select(sf => new SubstanceTreeItemViewModel(sf))).ToList();
            }
            else
            {
                var items = this.database.loadItems<SubstanceInfo>()
                                    .Where(x => x.Name.ToLower().Contains(this.filter))
                                    .Select(sf => new SubstanceTreeItemViewModel(sf)).ToList();

                var temp = this.database.loadItems<SubstanceFolder>()
                        .Where(f => items.FirstOrDefault(x => x.Parent_Id == f.Id) != null)
                        .Select(sf => new SubstanceTreeItemViewModel(sf)).ToList();

                IEnumerable<SubstanceTreeItemViewModel> folders = new List<SubstanceTreeItemViewModel>();

                while (temp.Count() != 0)
                {
                    folders = folders.Concat(temp);

                    temp = this.database.loadItems<SubstanceFolder>()
                        .Where(f => temp.FirstOrDefault(x => x.Parent_Id == f.Id) != null)
                        .Select(sf => new SubstanceTreeItemViewModel(sf)).ToList();
                }

                this.preloaded = items.Concat(folders).ToList();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public IEnumerable GetChildren(object parent)
        {
            IEnumerable<SubstanceTreeItemViewModel> result = null; //(new Collection<SubstanceTreeItemViewModel>()).AsEnumerable();

            if (parent == null)
            {
                result = this.preloaded.Where(s => s.Parent_Id == 0);
            }
            else if (parent is SubstanceTreeItemViewModel && ((SubstanceTreeItemViewModel)parent).Folder != null)
            {
                long parentId = ((SubstanceTreeItemViewModel)parent).Folder.Id;

                result = this.preloaded.Where(s => s.Parent_Id == parentId);
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public bool HasChildren(object parent)
        {
            if (parent == null)
                return false;

            return (parent is SubstanceTreeItemViewModel && ((SubstanceTreeItemViewModel)parent).Folder != null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<long> GetPathToItem(SubstanceTreeItemViewModel item)
        {
            List<long> res = new List<long>();

            var sf = this.database.getItemById<SubstanceFolder>(item.Parent_Id);

            while (sf != null)
            {
                res.Add(sf.Id);

                sf = this.database.getItemById<SubstanceFolder>(sf.Parent_Id);
            }

            res.Reverse();
            return res;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SubstanceInfo> GetFilteredSubstances()
        {
            return this.preloaded.Where(x => x.Substance != null).Select(s => s.Substance);
        }
    }
}
