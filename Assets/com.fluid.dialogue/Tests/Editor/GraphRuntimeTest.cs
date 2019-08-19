using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace FluidDialogue.Tests.Editor {
    public class GraphRuntimeTest {
        public class Constructor {
            [Test]
            public void It_should_set_the_root_to_the_root_runtime_value () {
                var root = Substitute.For<INodeData>();
                var rootCopy = Substitute.For<INode>();
                root.GetRuntime(null, null).ReturnsForAnyArgs(rootCopy);

                var graphData = Substitute.For<IGraphData>();
                IReadOnlyList<INodeData> nodeList = new List<INodeData> {root};
                graphData.Nodes.Returns(nodeList);
                graphData.Root.Returns(root);

                var graph = new GraphRuntime(null, graphData);

                Assert.AreEqual(rootCopy, graph.Root);
            }
        }

        public class GetCopyMethod {
            [Test]
            public void It_should_return_a_copy_of_the_original () {
                var root = Substitute.For<INodeData>();
                var rootCopy = Substitute.For<INode>();
                root.GetRuntime(null, null).ReturnsForAnyArgs(rootCopy);

                var graphData = Substitute.For<IGraphData>();
                IReadOnlyList<INodeData> nodeList = new List<INodeData> {root};
                graphData.Nodes.Returns(nodeList);
                graphData.Root.Returns(root);

                var graph = new GraphRuntime(null, graphData);

                Assert.AreEqual(rootCopy, graph.GetCopy(root));
            }
        }
    }
}
