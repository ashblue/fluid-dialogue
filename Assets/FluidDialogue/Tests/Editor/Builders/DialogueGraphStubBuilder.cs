using CleverCrow.Fluid.Dialogues.Graphs;
using NSubstitute;

namespace CleverCrow.Fluid.Dialogues.Builders {
    public class DialogueGraphStubBuilder {
        private IDialogueNode _next;

        public IDialogueGraph Build () {
            var graph = Substitute.For<IDialogueGraph>();
            graph.Root.Next().Returns(_next);

            return graph;
        }

        public DialogueGraphStubBuilder WithNextResult (IDialogueNode node) {
            _next = node;
            return this;
        }
    }
}
