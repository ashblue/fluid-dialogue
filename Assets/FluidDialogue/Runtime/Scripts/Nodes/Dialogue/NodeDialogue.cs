using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Conditions;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeDialogue : INode {
        private readonly List<INode> _childNodes;
        private readonly IActor _actor;
        private readonly string _dialogue;
        private readonly List<IChoice> _choices;
        private readonly List<ICondition> _conditions;

        private List<IChoice> _emittedChoices;

        public List<IAction> EnterActions { get; }
        public List<IAction> ExitActions { get; }
        public bool IsValid => _conditions.Find(c => !c.GetIsValid()) == null;

        public NodeDialogue (
            IActor actor,
            string dialogue,
            List<INode> children,
            List<IChoice> choices,
            List<ICondition> conditions,
            List<IAction> enterActions,
            List<IAction> exitActions
        ) {
            _actor = actor;
            _dialogue = dialogue;
            _childNodes = children;
            _choices = choices;
            _conditions = conditions;
            EnterActions = enterActions;
            ExitActions = exitActions;
        }

        private List<IChoice> GetValidChoices () {
            return _choices.Where(c => c.GetValidChildNode() != null).ToList();
        }

        public INode Next () {
            return _childNodes.Find(c => c.IsValid);
        }

        public void Play (IDialogueEvents events) {
            _emittedChoices = GetValidChoices();
            if (_emittedChoices.Count > 0) {
                events.Choice.Invoke(_actor, _dialogue, _emittedChoices);
                return;
            }

            events.Speak.Invoke(_actor, _dialogue);
        }

        public IChoice GetChoice (int index) {
            if (index >= _emittedChoices.Count) return null;

            return _emittedChoices[index];
        }
    }
}
