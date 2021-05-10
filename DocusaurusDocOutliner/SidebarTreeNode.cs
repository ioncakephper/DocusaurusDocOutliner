using System.Windows.Forms;

namespace DocusaurusDocOutliner
{
    public class SidebarTreeNode : TreeNode
    {
        public SidebarTreeNode(DocumentationSidebar sidebar): base()
        {
            Text = (!string.IsNullOrWhiteSpace(sidebar.Title) ? sidebar.Title : "Untitled");
            Tag = sidebar;
        }

        public DocumentationSidebar GetSidebar()
        {
            return (DocumentationSidebar)Tag;
        }
    }
}