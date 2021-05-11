using System;
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

        public SidebarTreeNode(DocumentationSidebar sidebar, ContextMenuStrip sidebarContextMenuStrip) : this(sidebar)
        {
            ContextMenuStrip = sidebarContextMenuStrip;
        }

        public DocumentationSidebar GetSidebar()
        {
            return (DocumentationSidebar)Tag;
        }

        internal DocumentationSidebar RetrieveSidebar()
        {
            DocumentationSidebar s = new DocumentationSidebar() { Title = Text };
            foreach (TopicTreeNode topicNode in Nodes)
            {
                DocumentationTopic topic = topicNode.RetrieveTopic();
                s.Topics.Add(topic);
            }

            return s;
        }
    }
}