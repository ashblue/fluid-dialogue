using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Conditions;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeDialogue : NodeBase {
        private readonly IActor _actor;
        private readonly string _dialogue;
        private readonly List<IChoice> _choices;

        private List<IChoice> _emittedChoices;

        public NodeDialogue (
            IActor actor,
            string dialogue,
            List<INode> children,
            List<IChoice> choices,
            List<ICondition> conditions,
            List<IAction> enterActions,
            List<IAction> exitActions) :
            base(children, conditions, enterActions, exitActions) {
            _actor = actor;
            _dialogue = dialogue;
            _choices = choices;
        }

        private List<IChoice> GetValidChoices () {
            var child = Next();
            if (child?.HubChoices != null && child.HubChoices.Count > 0) {
                return child.HubChoices;
            }

            return _choices.Where(c => c.GetValidChildNode() != null).ToList();
        }

        public override void Play (IDialoguePlayback playback) {
            _emittedChoices = GetValidChoices();
            if (_emittedChoices.Count > 0) {
                playback.Events.Choice.Invoke(_actor, _dialogue, _emittedChoices);
                return;
            }

            playback.Events.Speak.Invoke(_actor, _dialogue);
        }

        public override IChoice GetChoice (int index) {
            if (index >= _emittedChoices.Count) return null;

            return _emittedChoices[index];
        }
    }
}
