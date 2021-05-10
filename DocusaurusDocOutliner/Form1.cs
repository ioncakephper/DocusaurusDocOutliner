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
        private int _sidebarCount;
        private int _topicCount;

        public Form1()
        {
            InitializeComponent();
            AdjustSplitterWidth();
            Project = new DocumentationProject();
            Project.Sidebars.Add(new DocumentationSidebar() { Title = "Docs" });
        }

        private void AdjustSplitterWidth()
        {
            splitContainer1.SplitterDistance = (int)(splitContainer1.Width * 0.2);
        }

        public string FileName { get; private set; }
        public DocumentationProject Project { get; private set; }

        private void UpdateFormTitle()
        {
            string basename = (!string.IsNullOrWhiteSpace(FileName)) ? System.IO.Path.GetFileNameWithoutExtension(FileName) : "Untitled";
            string appname = "Docusaurus Documentation Project Outliner";

            Text = string.Format(@"{0} - {1}", basename, appname);
        }

        private void ScatterData()
        {
            PopulateTree();
            SetControlsEnabled();
        }

        private void PopulateTree()
        {
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(NewProjectNode(Project));
            treeView1.ExpandAll();
        }

        private ProjectTreeNode NewProjectNode(DocumentationProject project)
        {
            ProjectTreeNode projectNode = new ProjectTreeNode(project, projectContextMenuStrip);
            UpdateSidebars(project.Sidebars, projectNode);

            return projectNode;
        }

        private void UpdateSidebars(List<DocumentationSidebar> sidebars, TreeNode projectNode)
        {
            foreach (var sidebar in sidebars)
            {
                SidebarTreeNode sidebarNode = NewSidebarNode(sidebar);
                projectNode.Nodes.Add(sidebarNode);
            }
        }

        private SidebarTreeNode NewSidebarNode(DocumentationSidebar sidebar)
        {
            SidebarTreeNode sidebarNode = new SidebarTreeNode(sidebar, sidebarContextMenuStrip);
            UpdateTopics(sidebar.Topics, sidebarNode);

            return sidebarNode;
        }

        private void UpdateTopics(List<DocumentationTopic> topics, TreeNode sidebarNode)
        {
            foreach (var topic in topics)
            {
                TopicTreeNode topicNode = NewTopicNode(topic);
                sidebarNode.Nodes.Add(topicNode);
            }
        }

        private TopicTreeNode NewTopicNode(DocumentationTopic topic)
        {
            TopicTreeNode topicNode = new TopicTreeNode(topic, topicContextMenuStrip);
            UpdateTopics(topic.Topics, topicNode);

            return topicNode;
        }

        private void SetControlsEnabled()
        {
            // throw new NotImplementedException();
        }

        private void GatherData()
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ScatterData();
        }

        private void newSidebarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectTreeNode projectNode = (ProjectTreeNode)treeView1.TopNode;
            AddSidebarIntoProject(projectNode, NewSidebarNode(NewSidebar()));
        }

        private void AddSidebarIntoProject(ProjectTreeNode projectNode, SidebarTreeNode sidebarTreeNode)
        {
            projectNode.Nodes.Add(sidebarTreeNode);
            treeView1.SelectedNode = sidebarTreeNode;

        }

        private DocumentationSidebar NewSidebar()
        {
            return new DocumentationSidebar() { Title = "Sidebar " + _sidebarCount++.ToString() };
        }

        private void newTopicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SidebarTreeNode sidebarNode = (SidebarTreeNode)treeView1.SelectedNode;
            AddTopicIntoSidebar(sidebarNode, NewTopicNode(NewTopic()));
        }

        private void AddTopicIntoSidebar(SidebarTreeNode sidebarNode, TopicTreeNode topicTreeNode)
        {
            sidebarNode.Nodes.Add(topicTreeNode);
            treeView1.SelectedNode = topicTreeNode;
        }

        private DocumentationTopic NewTopic()
        {
            return new DocumentationTopic() { Title = "Topic " + _topicCount++.ToString() };

        }

        private void newTopicToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TopicTreeNode topicNode = (TopicTreeNode)treeView1.SelectedNode;
            AddTopicIntoTopic(topicNode, NewTopicNode(NewTopic()));
        }

        private void AddTopicIntoTopic(TopicTreeNode topicNode, TopicTreeNode topicTreeNode)
        {
            topicNode.Nodes.Add(topicTreeNode);
            treeView1.SelectedNode = topicTreeNode;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            weblidityFormCloser1.ConfirmFormClosing(sender, e);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (weblidityFormCloser1.Decision == DialogResult.Yes)
            {
                GatherData();
                weblidityFileOpenSave1.Save(FileName);
            }
        }
    }
}
