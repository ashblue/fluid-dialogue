using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Nodes;
using NSubstitute;

namespace CleverCrow.Fluid.Dialogues.Builders {
    public class DialogueNodeStubBuilder {
        private INodeRuntime _next;
        private readonly List<IAction> _exitActions = new List<IAction>();
        private readonly List<IAction> _enterActions = new List<IAction>();
        private readonly List<IChoiceRuntime> _choices = new List<IChoiceRuntime>();
        private bool _isValid = true;
        private INodeRuntime _clone;

        public DialogueNodeStubBuilder WithNextResult (INodeRuntime nodeRuntime) {
            _next = nodeRuntime;
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

        public DialogueNodeStubBuilder WithChoice (IChoiceRuntime choice) {
            _choices.Add(choice);
            return this;
        }

        public DialogueNodeStubBuilder WithIsValid (bool valid) {
            _isValid = valid;
            return this;
        }

        public INodeRuntime Build () {
            var node = Substitute.For<INodeRuntime>();
            node.Next().Returns(_next);
            node.ExitActions.Returns(_exitActions);
            node.EnterActions.Returns(_enterActions);
            node.IsValid.Returns(_isValid);

            for (var i = 0; i < _choices.Count; i++) {
                node.GetChoice(i).Returns(_choices[i]);
            }

            return node;
        }
    }
}
