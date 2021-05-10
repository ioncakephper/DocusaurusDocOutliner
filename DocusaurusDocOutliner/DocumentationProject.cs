using System.Collections.Generic;

namespace DocusaurusDocOutliner
{
    public class DocumentationProject
    {
        public string Title { get; internal set; }
        public List<DocumentationSidebar> Sidebars { get; internal set; }

        public DocumentationProject()
        {
            Sidebars = new List<DocumentationSidebar>();
        }
    }

}