using System.Collections.Generic;

namespace DocusaurusDocOutliner
{
    public class DocumentationTopic
    {
        public string Title { get;  set; }
        public List<DocumentationTopic> Topics { get;  set; }

        public DocumentationTopic()
        {
            Topics = new List<DocumentationTopic>();
        }
    }
}