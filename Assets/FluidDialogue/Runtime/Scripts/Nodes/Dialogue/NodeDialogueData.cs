using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Choices;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    [CreateNodeMenu("Dialogue")]
    public class NodeDialogueData : NodeDataBase {
        public ActorDefinition actor;

        [TextArea]
        public string dialogue;

        public List<ChoiceData> choices;
        public string nodeTitle;
        public override string DefaultName => "Dialogue";

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
