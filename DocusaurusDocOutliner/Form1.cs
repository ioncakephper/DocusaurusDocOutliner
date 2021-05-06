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
        public string FileName { get; private set; }

        public Form1()
        {
            InitializeComponent();
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
            // throw new NotImplementedException();
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
            string applicationName = "Docusaurus Documenation Outliner";
            Text = string.Format(@"{0} - {1}", baseName, applicationName);
        }
    }
}
