using System.Collections.Generic;

namespace DocusaurusDocOutliner
{
    public class DocumentationTopic
    {
        public string Title { get; internal set; }
        public List<DocumentationTopic> Topics { get; internal set; }

        public DocumentationTopic()
        {
            Topics = new List<DocumentationTopic>();
        }
    }
}