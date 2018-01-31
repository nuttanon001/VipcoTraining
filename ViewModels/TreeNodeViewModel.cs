using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace VipcoTraining.ViewModels
{
    public class TreeNodeViewModel<T> where T : class
    {
        // The data.
        public T data { get; private set; }
        // Child nodes in the tree.
        public List<TreeNodeViewModel<T>> children { get; private set; }


        #region Constructor
        public TreeNodeViewModel(T Parent)
        {
            this.data = Parent;
            this.children = new List<TreeNodeViewModel<T>>();
        }

        #endregion

        // Add a TreeNode to out Children list.
        public void AddChild(TreeNodeViewModel<T> child)
        {
            children.Add(child);
        }

    }
}
