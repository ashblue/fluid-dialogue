using CleverCrow.Fluid.Dialogues.Graphs;
using NSubstitute;

namespace CleverCrow.Fluid.Dialogues.Builders {
    public class DialogueNodeStubBuilder {
        private string _dialogue = "Lorem Ipsum";
        private IDialogueNode _next;

        public IDialogueNode Build () {
            var node = Substitute.For<IDialogueNode>();
            node.Dialogue.Returns(_dialogue);
            node.Next().Returns(_next);

            return node;
        }

        public DialogueNodeStubBuilder WithNextResult (IDialogueNode node) {
            _next = node;
            return this;
        }
    }
}
