using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes;
using NSubstitute;
using NUnit.Framework;

namespace FluidDialogue.Tests.Editor {
    public class GraphRuntimeTest {
        public class Constructor {
            [Test]
            public void It_should_set_the_root_to_the_root_runtime_value () {
                var root = Substitute.For<INodeData>();
                var graphData = Substitute.For<IGraphData>();
                graphData.Root.Returns(root);

                var graph = new GraphRuntime(graphData);

                Assert.IsNotNull(root.GetRuntime());
                Assert.AreEqual(root.GetRuntime(), graph.Root);
            }
        }
    }
}
