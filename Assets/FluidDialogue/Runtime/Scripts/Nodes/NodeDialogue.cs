using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Actions;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeDialogue : INodeRuntime {
        private readonly List<INodeRuntime> _childNodes;
        private readonly IActor _actor;
        private readonly string _dialogue;
        private List<IChoiceRuntime> _choices;

        public NodeDialogue (
            IActor actor,
            string dialogue,
            List<INodeRuntime> children,
            List<IChoiceRuntime> choices) {
            _actor = actor;
            _dialogue = dialogue;
            _childNodes = children;
            _choices = choices;
        }

        private List<IChoiceRuntime> GetValidChoices () {
            return _choices.Where(c => c.Node.IsValid).ToList();
        }

        public List<IAction> ExitActions { get; }
        public List<IAction> EnterActions { get; }

        // @TODO Checks that all conditions return valid
        public bool IsValid { get; }

        public INodeRuntime Next () {
            return _childNodes.Find(c => c.IsValid);
        }

        public void Play (IDialogueEvents events) {
            var choices = GetValidChoices();
            if (choices.Count > 0) {
                events.Choice.Invoke(_actor, _dialogue, choices);
                return;
            }

            events.Speak.Invoke(_actor, _dialogue);
        }

        public IChoiceRuntime GetChoice (int index) {
            throw new System.NotImplementedException();
//            return _choices.Where(c => c.Node.IsValid).ToList();
        }
    }
}
