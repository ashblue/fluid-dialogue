using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Builders;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeRootTest {
        public class NextMethod {
            [Test]
            public void It_should_return_a_valid_child () {
                var node = A.Node.Build();
                var nodeData = A.NodeData.WithNode(node).Build();
                var children = new List<INodeData> { nodeData };
                var graph = A.Graph.WithNode(nodeData).Build();

                var root = new NodeRoot(graph, null, children, null, null, null);

                Assert.AreEqual(node, root.Next());
            }

            [Test]
            public void It_should_not_return_an_invalid_child () {
                var child = A.Node.WithIsValid(false).Build();
                var childData = A.NodeData.WithNode(child).Build();
                var children = new List<INodeData> { childData };
                var graph = A.Graph.WithNode(childData).Build();

                var root = new NodeRoot(graph, null, children, null, null, null);

                Assert.AreEqual(null, root.Next());
            }
        }
    }
}
