using System;
using System.Collections.Generic;

namespace DocusaurusDocOutliner
{
    public class TopicTreeNode : AdjustedTreeNode<DocumentationTopic>
    {
        public override void UpdateNodeData(DocumentationTopic source)
        {
            base.UpdateNodeData(source);
            Text = (!string.IsNullOrWhiteSpace(source.Title)) ? source.Title : "Untitled";
            UpdateChildrenNode(source.Topics);
        }

        private void UpdateChildrenNode(List<DocumentationTopic> topics)
        {
            if (topics == null)
            {
                throw new ArgumentNullException(nameof(topics));
            }

            foreach (var topic in topics)
            {
                TopicTreeNode topicNode = SidebarTreeNode.NewTopicNode(topic);
                if (topicNode == null)
                {
                    throw new ArgumentNullException(nameof(topicNode));
                }
                Nodes.Add(topicNode);
            }
        }
    }
}