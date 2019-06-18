using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes;
using NSubstitute;

namespace CleverCrow.Fluid.Dialogues.Builders {
    public class DialogueGraphStubBuilder {
        private INodeRuntime _next;

        public IGraphRuntime Build () {
            var graph = Substitute.For<IGraphRuntime>();
            var root = A.Node
                .WithNextResult(_next)
                .Build();
            graph.Root.Returns(root);

            return graph;
        }

        public DialogueGraphStubBuilder WithNextResult (INodeRuntime nodeRuntime) {
            _next = nodeRuntime;
            return this;
        }
    }
}
