using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocusaurusDocOutlinerControlLibrary
{
    public partial class WeblidityFileOpenSave : Component
    {
        private SaveFileDialog _saveFileDialog;

        public event EventHandler<FileOpenSaveEventArgs> FileSave;
        public event EventHandler<FileOpenSaveEventArgs> AfterFileSave;
        public event EventHandler<FileOpenSaveEventArgs> BeforeFileSave;

        public SaveFileDialog SaveFileDialog
        {
            get
            {
                if (_saveFileDialog == null)
                {
                    return new SaveFileDialog();
                }
                return _saveFileDialog;
            }

            set => _saveFileDialog = value;
        }

        public WeblidityFileOpenSave()
        {
            InitializeComponent();
        }

        public WeblidityFileOpenSave(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public string FileName { get;  set; }

        public FileOpenSaveResult Save(string fileName)
        {
            FileName = fileName;
            return Save();
        }
        public FileOpenSaveResult Save()
        {
            if (string.IsNullOrWhiteSpace(FileName))
            {
                return SaveAs();
            }
            var e = new FileOpenSaveEventArgs() { FileName = FileName };
            OnFileSave(e);
            return e.Result;
        }

        protected void OnFileSave(FileOpenSaveEventArgs e)
        {
            OnBeforeFileSave(e);
            if (!e.Cancel)
            {
                if (FileSave != null)
                {
                    FileSave(this, e);
                    OnAfterFileSave(e);
                }
            }
        }

        private void OnBeforeFileSave(FileOpenSaveEventArgs e)
        {
            if (BeforeFileSave != null)
            {
                BeforeFileSave(this, e);
            }
        }

        private void OnAfterFileSave(FileOpenSaveEventArgs e)
        {
            if (AfterFileSave != null)
            {
                AfterFileSave(this, e);
            }
        }

        public FileOpenSaveResult SaveAs(string fileName)
        {
            FileName = fileName;
            return SaveAs();
        }

        public FileOpenSaveResult SaveAs()
        {
            SaveFileDialog d = SaveFileDialog;
            if (d.ShowDialog() == DialogResult.OK)
            {
                return Save(d.FileName);
            }
            return FileOpenSaveResult.Cancel;
        }
    }

    public class FileOpenSaveEventArgs: EventArgs
    {
        public FileOpenSaveEventArgs()
        {
            Result = FileOpenSaveResult.None;
        }

        public string FileName { get; set; }
        public FileOpenSaveResult Result { get;  set; }
        public bool Cancel { get; set; }
    }

    public enum FileOpenSaveResult
    {
        None,
        Cancel,
        Success,
        Errors
    }
}
