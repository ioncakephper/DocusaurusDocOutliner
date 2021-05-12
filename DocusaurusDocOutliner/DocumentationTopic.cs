using System.Collections.Generic;

namespace DocusaurusDocOutliner
{
    public class DocumentationTopic
    {
        private string _alternativeTitle;

        public string Title { get; internal set; }
        public List<DocumentationTopic> Topics { get; internal set; }
        public string AlternativeTitle {

            get {
                if (string.IsNullOrWhiteSpace(_alternativeTitle))
                {
                    return Title;
                }
                return _alternativeTitle;
            }

            internal set => _alternativeTitle = value; }

        public DocumentationTopic()
        {
            Topics = new List<DocumentationTopic>();
        }
    }
}