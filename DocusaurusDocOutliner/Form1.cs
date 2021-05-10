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
        public Form1()
        {
            InitializeComponent();
            Project = new DocumentationProject();
            Project.Sidebars.Add(new DocumentationSidebar() { Title = "Docs" });
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
            ProjectTreeNode projectNode = new ProjectTreeNode(project);
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
            SidebarTreeNode sidebarNode = new SidebarTreeNode(sidebar);
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
            TopicTreeNode topicNode = new TopicTreeNode(topic);
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
    }
}
