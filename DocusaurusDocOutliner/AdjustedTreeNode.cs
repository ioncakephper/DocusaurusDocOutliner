using System;
using System.Windows.Forms;

namespace DocusaurusDocOutliner
{
    public class AdjustedTreeNode<T> : TreeNode
    {
        public virtual void UpdateNodeData(T source)
        {
        }
    }
}