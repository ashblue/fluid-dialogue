using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Builders;
using CleverCrow.Fluid.Dialogues.Nodes;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Choices.Tests {
    public class ChoiceRuntimeTest {
        public class GetValidChildNodeMethod {
            [Test]
            public void It_should_return_a_valid_child () {
                var node = A.Node.Build();
                var nodeData = A.NodeData.WithNode(node).Build();
                var children = new List<INodeData> { nodeData };
                var graph = A.Graph.WithNode(nodeData).Build();
                var choice = new ChoiceRuntime(graph, "", null, children);

                var result = choice.GetValidChildNode();

                Assert.AreEqual(node, result);
            }

            [Test]
            public void It_should_not_return_an_invalid_child () {
                var node = A.Node.WithIsValid(false).Build();
                var nodeData = A.NodeData.WithNode(node).Build();
                var children = new List<INodeData> { nodeData };
                var graph = A.Graph.WithNode(nodeData).Build();
                var choice = new ChoiceRuntime(graph, "", null, children);

                var result = choice.GetValidChildNode();

                Assert.IsNull(result);
            }
        }
    }
}
