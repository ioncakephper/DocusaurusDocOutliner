using System;
using System.Windows.Forms;

namespace DocusaurusDocOutliner
{
    public class TopicTreeNode: TreeNode
    {
        public TopicTreeNode(DocumentationTopic topic): base()
        {
            Text = (!string.IsNullOrWhiteSpace(topic.Title) ? topic.Title : "Untitled");
            Tag = topic;
        }

        public TopicTreeNode(DocumentationTopic topic, ContextMenuStrip topicContextMenuStrip) : this(topic)
        {
            ContextMenuStrip = topicContextMenuStrip;
        }

        public DocumentationTopic GetTopic()
        {
            return (DocumentationTopic)Tag;
        }

        internal DocumentationTopic RetrieveTopic()
        {
            DocumentationTopic t = new DocumentationTopic() { Title = Text };
            foreach (TopicTreeNode topicNode in Nodes)
            {
                DocumentationTopic topic = topicNode.RetrieveTopic();
                t.Topics.Add(topic);
            }
            return t;
        }
    }
}