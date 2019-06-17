using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Actions;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class DialogueNode : DialogueNodeBase {
        public List<DialogueNodeBase> children = new List<DialogueNodeBase>();
        public List<Choice> choices = new List<Choice>();

        public override string Dialogue { get; }
        public override IActor Actor { get; }
        public override List<IAction> ExitActions { get; }
        public override List<IAction> EnterActions { get; }
        public override bool IsValid { get; }

        public override IDialogueNode Next () {
            return DialogueNodeInternal.GetValidChild(children.ToList<IDialogueNode>());
        }

        public override List<IChoice> GetChoices () {
            return DialogueNodeInternal.GetValidChoices(choices.ToList<IChoice>());
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
