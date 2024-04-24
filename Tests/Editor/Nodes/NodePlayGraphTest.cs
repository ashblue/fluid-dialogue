using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes.PlayGraph;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodePlayGraphTest {
        public class PlayMethod {
            [Test]
            public void It_should_call_PlayChild_on_the_DialogueController () {
                var graph = Substitute.For<IGraphData>();
                var playGraph = new NodePlayGraph(null, null, graph, null, null, null, null);
                var playback = Substitute.For<IDialoguePlayback>();

                playGraph.Play(playback);

                playback.ParentCtrl.Received(1).PlayChild(graph);
            }
        }
    }
}
