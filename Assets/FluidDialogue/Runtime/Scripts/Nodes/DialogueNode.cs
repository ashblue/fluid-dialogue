using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Actions;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class DialogueNode : DialogueNodeBase {
        private List<IChoice> _validChoices;
        private List<DialogueNodeBase> _children = new List<DialogueNodeBase>();
        private List<Choice> _choices = new List<Choice>();
        private IActor _actor;
        private string _dialogue;

        public override List<IAction> ExitActions { get; }
        public override List<IAction> EnterActions { get; }
        public override bool IsValid { get; }

        public override IDialogueNode Next () {
            return DialogueNodeInternal.GetValidChild(_children.ToList<IDialogueNode>());
        }

        public override void Play (DialoguePlayback playback) {
            _validChoices = DialogueNodeInternal.GetValidChoices(_choices.ToList<IChoice>());
            if (_validChoices.Count > 0) {
                playback.Events.Choice.Invoke(_actor, _dialogue, _validChoices);
                return;
            }

            playback.Events.Speak.Invoke(_actor, _dialogue);
        }

        public override IChoice GetChoice (int index) {
            return _validChoices[index];
        }
    }

    public static class DialogueNodeInternal {
        public static IDialogueNode GetValidChild (List<IDialogueNode> children) {
            return children.Find(c => c.IsValid);
        }

        public static List<IChoice> GetValidChoices (List<IChoice> choices) {
            return choices.Where(c => c.Node.IsValid).ToList();
        }
    }
}
