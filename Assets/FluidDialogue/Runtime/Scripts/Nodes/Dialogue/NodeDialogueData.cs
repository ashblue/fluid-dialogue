using System.Linq;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    [CreateNodeMenu("Dialogue", 1)]
    public class NodeDialogueData : NodeDataChoiceBase {
        public ActorDefinition actor;

        [TextArea]
        public string dialogue;

        protected override string DefaultName => "Dialogue";

        public override INode GetRuntime (IDialogueController controller) {
            return new NodeDialogue(
                UniqueId,
                actor,
                dialogue,
                children.Select(c => c.GetRuntime(controller)).ToList(),
                choices.Select(c => c.GetRuntime(controller)).ToList(),
                conditions.Select(c => c.GetRuntime(controller)).ToList(),
                enterActions.Select(a => a.GetRuntime(controller)).ToList(),
                exitActions.Select(a => a.GetRuntime(controller)).ToList()
            );
        }
    }
}
