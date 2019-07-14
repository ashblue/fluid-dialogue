using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes;
using NSubstitute;

namespace CleverCrow.Fluid.Dialogues.Builders {
    public class DialogueGraphStubBuilder {
        private INode _next;

        public IGraph Build () {
            var graph = Substitute.For<IGraph>();
            var root = A.Node
                .WithNextResult(_next)
                .WithPlayAction((playback) => playback.Next())
                .Build();
            graph.Root.Returns(root);

            return graph;
        }

        public DialogueGraphStubBuilder WithNextResult (INode node) {
            _next = node;
            return this;
        }
    }
}
