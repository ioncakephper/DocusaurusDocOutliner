using System;
using System.Collections.Generic;

namespace DocusaurusDocOutliner
{
    public class SidebarTreeNode : AdjustedTreeNode<DocumentationSidebar>
    {
        public override void UpdateNodeData(DocumentationSidebar source)
        {
            base.UpdateNodeData(source);
            Tag = source;
            Text = (!string.IsNullOrWhiteSpace(source.Title)) ? source.Title : "Untitled";
            UpdateChildrenNodes(source.Topics);
        }

        private void UpdateChildrenNodes(List<DocumentationTopic> topics)
        {
            if (topics == null)
            {
                throw new ArgumentNullException(nameof(topics));
            }

            foreach (var topic in topics)
            {
                TopicTreeNode topicNode = NewTopicNode(topic);
                if (topicNode == null)
                {
                    throw new ArgumentNullException(nameof(topic));
                }
            }
        }

        public static TopicTreeNode NewTopicNode(DocumentationTopic topic)
        {
            if (topic == null)
            {
                throw new ArgumentNullException(nameof(topic));
            }

            TopicTreeNode topicNode = new TopicTreeNode();
            if (topicNode == null)
            {
                throw new ArgumentNullException(nameof(topicNode));
            }
            topicNode.UpdateNodeData(topic);

            return topicNode;
        }
    }
}