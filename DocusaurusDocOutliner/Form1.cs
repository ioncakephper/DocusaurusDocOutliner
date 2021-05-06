using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocusaurusDocOutliner
{
    public partial class Form1 : Form
    {
        private int _topicCount;

        public string FileName { get; set; }
        public DocumentationProject Project { get; set; }

        public Form1()
        {
            InitializeComponent();
            Project = new DocumentationProject() { Sidebars = new DocumentationSidebar[] { new DocumentationSidebar { Title = "Docs" } } };
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var d = new AboutBox();
            d.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ScatterData();
            UpdateFormText();
        }

        private void ScatterData()
        {
            PopulateTree();
            SetControlsEnabled();
        }

        private void PopulateTree()
        {
            treeView1.Nodes.Clear();
            ProjectTreeNode projectNode = NewProjectTreeNode(Project);
            if (projectNode != null)
            {
                treeView1.Nodes.Add(projectNode);
                treeView1.SelectedNode = projectNode;
                treeView1.ExpandAll();
            }
        }

        private ProjectTreeNode NewProjectTreeNode(DocumentationProject project)
        {
            ProjectTreeNode projectNode = new ProjectTreeNode();
            projectNode.SetTextAndTag(project);
            UpdateProjectChildrenNodes(project.Sidebars, projectNode);
            projectNode.ContextMenuStrip = projectContextMenuStrip;

            return projectNode;
        }

        private void UpdateProjectChildrenNodes(DocumentationSidebar[] sidebars, ProjectTreeNode projectNode)
        {
            if (sidebars == null)
            {
                throw new ArgumentNullException(nameof(sidebars));
            }

            if (projectNode == null)
            {
                throw new ArgumentNullException(nameof(projectNode));
            }

            foreach (var sidebar in sidebars)
            {
                SidebarTreeNode sidebarNode = NewSidebarNode(sidebar);
                if (sidebarNode != null)
                {
                    projectNode.Nodes.Add(sidebarNode);
                }
            }
        }

        private SidebarTreeNode NewSidebarNode(DocumentationSidebar sidebar)
        {
            SidebarTreeNode sidebarNode = new SidebarTreeNode();
            sidebarNode.SetTextAndTag(sidebar);
            UpdateSidebarChildrenNodes(sidebar.Topics, sidebarNode);

            sidebarNode.ContextMenuStrip = sidebarContextMenuStrip;
            return sidebarNode;

        }

        private void UpdateSidebarChildrenNodes(DocumentationTopic[] topics, SidebarTreeNode sidebarNode)
        {
            if (topics == null)
            {
                throw new ArgumentNullException(nameof(topics));
            }

            if (sidebarNode == null)
            {
                throw new ArgumentNullException(nameof(sidebarNode));
            }

            foreach (var topic in topics)
            {
                TopicTreeNode topicNode = NewTopicNode(topic);
                if (topicNode != null)
                {
                    sidebarNode.Nodes.Add(topicNode);
                }
            }
        }

        private TopicTreeNode NewTopicNode(DocumentationTopic topic)
        {
            TopicTreeNode topicNode = new TopicTreeNode();
            topicNode.SetTextAndTag(topic);
            UpdateTopicChildrenNodes(topic.Topics, topicNode);

            topicNode.ContextMenuStrip = topicContextMenuStrip;
            return topicNode;
        }

        private void UpdateTopicChildrenNodes(DocumentationTopic[] topics, TopicTreeNode topicNode)
        {
            if (topics == null)
            {
                throw new ArgumentNullException(nameof(topics));
            }

            if (topicNode == null)
            {
                throw new ArgumentNullException(nameof(topicNode));
            }

            foreach (var topic in topics)
            {
                TopicTreeNode subTopicNode = NewTopicNode(topic);
                if (subTopicNode != null)
                {
                    topicNode.Nodes.Add(subTopicNode);
                }
            }
        }

        private void SetControlsEnabled()
        {
            
        }

        private void GatherData()
        {

        }

        private void UpdateFormText()
        {
            string baseName = (!string.IsNullOrWhiteSpace(FileName)) ? System.IO.Path.GetFileNameWithoutExtension(FileName) : "Untitled";
            string applicationName = "Docusaurus Documentation Outliner";
            Text = string.Format(@"{0} - {1}", baseName, applicationName);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void newTopicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTopicInSidebar(treeView1.SelectedNode, NewTopicNode(NewTopic()));
        }

        private void AddTopicInSidebar(TreeNode selectedNode, TopicTreeNode topicTreeNode)
        {
            if (selectedNode == null)
            {
                throw new ArgumentNullException(nameof(selectedNode));
            }

            if (topicTreeNode == null)
            {
                throw new ArgumentNullException(nameof(topicTreeNode));
            }

            selectedNode.Nodes.Add(topicTreeNode);
            treeView1.SelectedNode = topicTreeNode;
        }

        private DocumentationTopic NewTopic()
        {
            return new DocumentationTopic() { Title = "Topic " + _topicCount++.ToString() };
        }

        private void newTopicToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddTopicInTopic(treeView1.SelectedNode, NewTopicNode(NewTopic()));
        }

        private void AddTopicInTopic(TreeNode selectedNode, TopicTreeNode topicTreeNode)
        {
            if (topicTreeNode == null)
            {
                throw new ArgumentNullException(nameof(topicTreeNode));
            }

            selectedNode.Nodes.Add(topicTreeNode);
            treeView1.SelectedNode = topicTreeNode;
        }
    }

    internal class TopicTreeNode : TreeNode
    {
        internal void SetTextAndTag(DocumentationTopic topic)
        {
            Text = (!string.IsNullOrWhiteSpace(topic.Title)) ? topic.Title.Trim() : "Untitled";
            Tag = topic;
        }
    }

    internal class SidebarTreeNode : TreeNode
    {
        internal void SetTextAndTag(DocumentationSidebar sidebar)
        {
            Text = (!string.IsNullOrWhiteSpace(sidebar.Title)) ? sidebar.Title.Trim() : "Untitled";
            Tag = sidebar;
        }
    }

    internal class ProjectTreeNode : TreeNode
    {
        public void SetTextAndTag(DocumentationProject project)
        {
            Text = (!string.IsNullOrWhiteSpace(project.Title)) ? project.Title.Trim() : "Untitled";
            Tag = project;
        }
    }
}
