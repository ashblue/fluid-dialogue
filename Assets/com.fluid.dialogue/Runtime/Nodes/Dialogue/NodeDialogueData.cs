using System.Linq;
using CleverCrow.Fluid.Dialogues.Graphs;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    [CreateMenu("Dialogue", 1)]
    public class NodeDialogueData : NodeDataChoiceBase {
        public ActorDefinition actor;

        [TextArea]
        public string dialogue;

        protected override string DefaultName => "Dialogue";
        public override string Text => dialogue;

        public override INode GetRuntime (IGraph graphRuntime, IDialogueController controller) {
            return new NodeDialogue(
                graphRuntime,
                UniqueId,
                actor,
                dialogue,
                children.ToList<INodeData>(),
                choices.Select(c => c.GetRuntime(graphRuntime, controller)).ToList(),
                conditions.Select(c => c.GetRuntime(graphRuntime, controller)).ToList(),
                enterActions.Select(a => a.GetRuntime(graphRuntime, controller)).ToList(),
                exitActions.Select(a => a.GetRuntime(graphRuntime, controller)).ToList()
            );
        }
    }
}
