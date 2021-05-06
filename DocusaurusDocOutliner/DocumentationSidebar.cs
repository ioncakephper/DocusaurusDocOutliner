using System.Collections.Generic;

namespace DocusaurusDocOutliner
{
    public class DocumentationSidebar
    {
        public string Title { get; internal set; }
        public List<DocumentationTopic> Topics { get; set; }

        public DocumentationSidebar()
        {
            Topics = new List<DocumentationTopic>();
        }
    }
}