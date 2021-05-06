using System;
using System.Collections.Generic;

namespace DocusaurusDocOutliner
{
    public class ProjectTreeNode : AdjustedTreeNode<DocumentationProject>
    {
        public override void UpdateNodeData(DocumentationProject source)
        {
            base.UpdateNodeData(source);
            Tag = source;
            Text = (!string.IsNullOrWhiteSpace(source.Title)) ? source.Title : "Untitled";
            UpdateChildrenNodes(source.Sidebars);
        }

        public void AddSidebarToProject(SidebarTreeNode sidebarNode)
        {
            DocumentationSidebar sb = ((DocumentationSidebar)sidebarNode.Tag);
            ((DocumentationProject)Tag).Sidebars.Add(sb);
        }

        private void UpdateChildrenNodes(List<DocumentationSidebar> sidebars)
        {
            if (sidebars == null)
            {
                throw new ArgumentNullException(nameof(sidebars));
            }

            foreach (var sidebar in sidebars)
            {
                SidebarTreeNode sidebarNode = NewSidebarNode(sidebar);
                if (sidebarNode == null)
                {
                    throw new ArgumentNullException(nameof(sidebarNode));
                }
                Nodes.Add(sidebarNode);
            }
        }

        public static SidebarTreeNode NewSidebarNode(DocumentationSidebar sidebar)
        {
            SidebarTreeNode sidebarNode = new SidebarTreeNode();
            sidebarNode.UpdateNodeData(sidebar);

            return sidebarNode;
        }
    }
}