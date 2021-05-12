using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocusaurusDocOutlinerControlLibrary;

namespace DocusaurusDocOutliner
{
    public partial class Form1 : Form
    {

        public event EventHandler<ApplicationFileNameChangedEventArgs> ApplicationFileNameChanged;

        private int _sidebarCount;
        private int _topicCount;

        public Form1()
        {
            InitializeComponent();
            ApplicationFileNameChanged += Form1_ApplicationFileNameChanged;
            AdjustSplitterWidth();
            Project = new DocumentationProject();
            Project.Sidebars.Add(new DocumentationSidebar() { Title = "Docs" });
        }

        private void Form1_ApplicationFileNameChanged(object sender, ApplicationFileNameChangedEventArgs e)
        {
            UpdateFormTitle();
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
            SetBuildControlsEnabled();
            SetMoveTopicsControlsEnabled();
        }

        private void SetMoveTopicsControlsEnabled()
        {
            var buildControlsList = new ControlsToEnableList();
            // buildControlsList.AddRange(new object[] { buildToolStripButton, buildToolStripMenuItem });

            buildControlsList.SetControlsEnabled();
        }

        private void SetBuildControlsEnabled()
        {
            var buildControlsList = new ControlsToEnableList();
            buildControlsList.AddRange(new object[] { buildToolStripButton, buildToolStripMenuItem });

            buildControlsList.SetControlsEnabled();
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
            weblidityFormCloser1.IsDirty = true;
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
            AddTopicIntoParent(sidebarNode, topicTreeNode);
        }

        private void AddTopicIntoParent(TreeNode sidebarNode, TreeNode topicTreeNode)
        {
            sidebarNode.Nodes.Add(topicTreeNode);
            treeView1.SelectedNode = topicTreeNode;
            weblidityFormCloser1.IsDirty = true;
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
            AddTopicIntoParent(topicNode, topicTreeNode);
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

        private void buildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DocumentationProject project = ((ProjectTreeNode)treeView1.TopNode).RetrieveProject();

            // Generate sidebars.js in Docusaurus website folder           
            var sidebarsRuntimeTextTemplate = new SidebarsRuntimeTextTemplate();           
            sidebarsRuntimeTextTemplate.Content = ProjectSidebarsAsJson(project);

            string sidebarsText = sidebarsRuntimeTextTemplate.TransformText();
            string sidebarsFilename = "sidebars.js";

            File.WriteAllText(sidebarsFilename, sidebarsText);

            MessageBox.Show(string.Format(@"Sidebars file created."), "Docusaurus Documentation Project Outliner", MessageBoxButtons.OK);
        }

        private string ProjectSidebarsAsJson(DocumentationProject project)
        {
            if (project == null)
            {
                return new JsonObject().ToString();
            }

            JsonObject jo = new JsonObject();
            foreach (var sidebar in project.Sidebars)
            {
                jo.Add(Surround(sidebar.Title), SidebarJsonTopics(sidebar.Topics));
            }
            return jo.ToString();
        }

        private string SidebarJsonTopics(List<DocumentationTopic> topics)
        {
            if (topics == null)
            {
                return new JsonArray().ToString();
            }

            JsonArray ja = new JsonArray();
            foreach (var topic in topics)
            {
                string itemToAdd = (topic.Topics.Count == 0) ? Surround(TopicSlug(topic)) : MultiLevelTopic(topic);
                ja.Add(itemToAdd);
            }
            return ja.ToString();
        }

        private string TopicSlug(DocumentationTopic topic)
        {
            return topic.Title.Trim().Replace(' ', '-').ToLower();
        }

        private string MultiLevelTopic(DocumentationTopic topic)
        {
            JsonObject js = new JsonObject();
            js.Add(Surround("type"), Surround("category"));
            js.Add(Surround("label"), Surround(topic.Title));
            js.Add(Surround("items"), SidebarJsonTopics(topic, topic.Topics));
            return js.ToString();
        }

        private string SidebarJsonTopics(DocumentationTopic topic, List<DocumentationTopic> topics)
        {
            var extraTopics = new List<DocumentationTopic>();
            extraTopics.Add(new DocumentationTopic() { Title = topic.Title, AlternativeTitle = "Overview" });
            extraTopics.AddRange(topics);

            return SidebarJsonTopics(extraTopics);
        }

        private string Surround(string v)
        {
            return Surround("\"", v);
        }

        private string Surround(string encapsulator, string s)
        {
            return string.Format(@"{0}{1}{0}", encapsulator, s);
        }

        private void buildToolStripButton_Click(object sender, EventArgs e)
        {
            buildToolStripMenuItem_Click(sender, e);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var d = new AboutBox1();
            d.ShowDialog();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var d = new OptionsDialog();
            d.ShowDialog();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            var result = weblidityFileOpenSave1.Save(FileName);
            UpdateApplicationFileName(result);
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToolStripButton_Click(sender, e);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = weblidityFileOpenSave1.SaveAs(FileName);
            UpdateApplicationFileName(result);
        }

        private void UpdateApplicationFileName(FileOpenSaveResult result)
        {
            if (result == DocusaurusDocOutlinerControlLibrary.FileOpenSaveResult.Success)
            {
                FileName = weblidityFileOpenSave1.FileName;
                weblidityFormCloser1.IsDirty = false;
                OnApplicationFileNameChanged(FileName);
            }
        }

        protected void OnApplicationFileNameChanged(string fileName)
        {
            if (ApplicationFileNameChanged != null)
            {
                ApplicationFileNameChanged(this, new ApplicationFileNameChangedEventArgs() { FileName = fileName });
            }
        }

        private void weblidityFileOpenSave1_FileSave(object sender, FileOpenSaveEventArgs e)
        {
            e.Result = FileOpenSaveResult.Success;
        }
    }

    internal class ControlsToEnableList: List<object>
    {
        public ControlsToEnableList()
        {
        }

        internal void SetControlsEnabled()
        {
            // throw new NotImplementedException();
        }
    }

    internal class JsonArray
    {
        public List<string> Items { get; private set; }

        public JsonArray()
        {
            Items = new List<string>();
        }

        internal void Add(string v)
        {
            Items.Add(v);
        }

        public override string ToString()
        {
            string[] arr = Items.ToArray();
            return string.Format(@"[ {0} ]", string.Join(",", arr));
        }
    }

    internal class JsonObject
    {
        public List<Tuple<string, string>> Items { get; private set; }

        public JsonObject()
        {
            Items = new List<Tuple<string, string>>();
        }

        internal void Add(string s, string v)
        {
            Items.Add(new Tuple<string, string>(s, v));
        }

        public override string ToString()
        {
            string[] arr = Items.ToArray<Tuple<string, string>>().Select(p => string.Format(@"{0}:{1}", p.Item1, p.Item2)).ToArray();
            StringBuilder sb = new StringBuilder();
            sb.Append("{ ");
            sb.AppendFormat(@"{0}", string.Join(", ", arr));
            sb.Append(" }");

            return sb.ToString();
        }
    }

    public partial class SidebarsRuntimeTextTemplate
    {
        public string Content { get; set; }
    }
}
