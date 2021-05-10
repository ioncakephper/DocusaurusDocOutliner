using System.Windows.Forms;

namespace DocusaurusDocOutliner
{
    public class ProjectTreeNode : TreeNode
    {
        public ProjectTreeNode(DocumentationProject project): base()
        {
            Text = (!string.IsNullOrWhiteSpace(project.Title) ? project.Title : "Untitled");
            Tag = project;
        }

        public DocumentationProject GetProject()
        {
            return (DocumentationProject)Tag;
        }
    }
}