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
    }

    public class FileNameChangedEventArgs : EventArgs
    {
        public string NewFileName { get; internal set; }
    }
}
