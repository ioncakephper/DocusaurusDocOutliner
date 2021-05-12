using System;

namespace DocusaurusDocOutliner
{
    public class ApplicationFileNameChangedEventArgs : EventArgs
    {
        public string FileName { get; internal set; }
    }
}