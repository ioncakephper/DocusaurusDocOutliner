namespace DocusaurusDocOutliner
{
    public class DocumentationSidebar
    {
        public DocumentationTopic[] Topics { get; set; }
        public string Title { get; set; }

        public DocumentationSidebar()
        {
            Topics = new DocumentationTopic[] { };
        }
    }
}