using System;
using System.Windows.Forms;
using System.Text.Json;
using System.IO;

namespace DocusaurusDocOutliner
{
    public partial class Form1 : Form
    {
        private string _fileName;

        public event EventHandler<FileNameChangedEventArgs> FileNameChanged;

        public string FileName {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                OnFileNameChanged(_fileName);
            }
        }

        public DocumentationProject Project { get;  set; }

        protected void OnFileNameChanged(string fileName)
        {
            if (FileNameChanged != null)
            {
                FileNameChangedEventArgs e = new FileNameChangedEventArgs() { NewFileName = fileName };
                FileNameChanged(this, e);
            }
        }

        public Form1()
        {
            InitializeComponent();
            Project = new DocumentationProject();
            Project.Sidebars.Add(new DocumentationSidebar() { Title = "Docs" });

            FileNameChanged += Form1_FileNameChanged;            
        }

        private void Form1_FileNameChanged(object sender, FileNameChangedEventArgs e)
        {
            UpdateFormText();
        }

        public void ScatterData()
        {
            SetControlsEnabled();
        }

        private void SetControlsEnabled()
        {
            // throw new NotImplementedException();
        }

        private void GatherData()
        {

        }

        private void UpdateFormText()
        {
            string baseName = (!string.IsNullOrWhiteSpace(FileName) ? System.IO.Path.GetFileNameWithoutExtension(FileName) : "Untitled");
            string appName = "Docusaurus Documentation Project Outliner";
            Text = string.Format(@"{0} - {1}", baseName, appName);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ScatterData();
            weblidityFormCloser1.IsDirty = true;
            UpdateFormText();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            weblidityFormCloser1.ConfirmFormClosing(sender, e);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var d = new AboutBox1();
            d.ShowDialog();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (weblidityFormCloser1.Decision == DialogResult.Yes)
            {
                GatherData();
                // Save data to file...
                var resultSave = weblidityFileOpenSave1.Save();
            }
        }

        private void weblidityFileOpenSave1_FileSave(object sender, DocusaurusDocOutlinerControlLibrary.FileOpenSaveEventArgs e)
        {
            MessageBox.Show(string.Format(@"Saving into {0}", e.FileName));
            e.Result = DocusaurusDocOutlinerControlLibrary.FileOpenSaveResult.Success;
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            var result = weblidityFileOpenSave1.Save(FileName);
            if (result == DocusaurusDocOutlinerControlLibrary.FileOpenSaveResult.Success)
            {
                FileName = weblidityFileOpenSave1.FileName;
                weblidityFormCloser1.IsDirty = false;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToolStripButton_Click(sender, e);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = weblidityFileOpenSave1.SaveAs(FileName);
            if (result == DocusaurusDocOutlinerControlLibrary.FileOpenSaveResult.Success)
            {
                FileName = weblidityFileOpenSave1.FileName;
                weblidityFormCloser1.IsDirty = false;
            }
        }

        private void buildToolStripButton_Click(object sender, EventArgs e)
        {
            GatherData();
            string output = JsonSerializer.Serialize(Project, new JsonSerializerOptions() { WriteIndented = true});
            File.WriteAllText("myfile.json", output);
        }
    }

    public class FileNameChangedEventArgs : EventArgs
    {
        public string NewFileName { get; internal set; }
    }
}
