using CleverCrow.Fluid.Dialogues.Graphs;
using NSubstitute;

namespace CleverCrow.Fluid.Dialogues.Builders {
    public class DialogueGraphStubBuilder {
        private IDialogueNode _next;

        public IDialogueGraph Build () {
            var graph = Substitute.For<IDialogueGraph>();
            var root = A.Node
                .WithNextResult(_next)
                .Build();
            graph.Root.Returns(root);

            return graph;
        }

        public DialogueGraphStubBuilder WithNextResult (IDialogueNode node) {
            _next = node;
            return this;
        }
    }
}
