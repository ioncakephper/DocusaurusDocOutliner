﻿using System.Windows.Forms;

namespace DocusaurusDocOutliner
{
    public class TopicTreeNode: TreeNode
    {
        public TopicTreeNode(DocumentationTopic topic): base()
        {
            Text = (!string.IsNullOrWhiteSpace(topic.Title) ? topic.Title : "Untitled");
            Tag = topic;
        }

        public DocumentationTopic GetTopic()
        {
            return (DocumentationTopic)Tag;
        }
    }
}