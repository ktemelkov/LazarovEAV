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
    class SubstanceTreeMockProvider : ITreeModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public IEnumerable GetChildren(object parent)
        {
            if (parent == null)
            {
                return new ObservableCollection<object>(){ new SubstanceFolder(){ Name = "Test 1"} };
            }

            if (parent is SubstanceFolder)
            {
                return new ObservableCollection<object>() 
                    { 
                        new SubstanceInfo() { Name = "Substance 1", Type = 0 },
                        new SubstanceInfo() { Name = "Substance 2", Type = 0 },
                        new SubstanceInfo() { Name = "Substance 3", Type = 0 } 
                    };
            }

            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public bool HasChildren(object parent)
        {
            return (parent == null || parent is SubstanceFolder);
        }
    }
}
