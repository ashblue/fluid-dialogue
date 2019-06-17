using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Graphs;
using NSubstitute;

namespace CleverCrow.Fluid.Dialogues.Builders {
    public class DialogueNodeStubBuilder {
        private string _dialogue = "Lorem Ipsum";
        private IDialogueNode _next;
        private readonly List<IAction> _exitActions = new List<IAction>();
        private readonly List<IAction> _enterActions = new List<IAction>();
        private readonly List<IChoice> _choices = new List<IChoice>();

        public DialogueNodeStubBuilder WithNextResult (IDialogueNode node) {
            _next = node;
            return this;
        }

        public DialogueNodeStubBuilder WithEnterAction (IAction action) {
            _enterActions.Add(action);
            return this;
        }

        public DialogueNodeStubBuilder WithExitAction (IAction action) {
            _exitActions.Add(action);
            return this;
        }

        public DialogueNodeStubBuilder WithChoice (IChoice choice) {
            _choices.Add(choice);
            return this;
        }

        public IDialogueNode Build () {
            var node = Substitute.For<IDialogueNode>();
            node.Dialogue.Returns(_dialogue);
            node.Next().Returns(_next);
            node.ExitActions.Returns(_exitActions);
            node.EnterActions.Returns(_enterActions);
            node.GetChoices().Returns(_choices);

            return node;
        }
    }
}
