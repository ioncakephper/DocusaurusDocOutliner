namespace DocusaurusDocOutliner
{
    public class DocumentationTopic
    {
        public DocumentationTopic[] Topics { get; set; }
        public string Title { get; internal set; }

        public DocumentationTopic()
        {
            Topics = new DocumentationTopic[] { };
        }
    }
}