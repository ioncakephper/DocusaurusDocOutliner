namespace DocusaurusDocOutliner
{
    public class DocumentationProject
    {
        public string Title { get; set; }
        public DocumentationSidebar[] Sidebars { get;  set; }

        public DocumentationProject()
        {
            Sidebars = new DocumentationSidebar[] { };
        }
    }
}