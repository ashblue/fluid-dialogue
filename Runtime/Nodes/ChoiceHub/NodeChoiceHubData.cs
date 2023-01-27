using System.Linq;
using CleverCrow.Fluid.Dialogues.Graphs;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    [CreateMenu("Hub/Choice")]
    public class NodeChoiceHubData : NodeDataChoiceBase {
        protected override string DefaultName => "Choice Hub";
        public override bool HideInspectorActions => true;

        public override INode GetRuntime (IGraph graphRuntime, IDialogueController dialogue) {
            var runtimeChoices = choices.Select(c => c.GetRuntime(graphRuntime, dialogue)).ToList();
            return new NodeChoiceHub(
                UniqueId,
                runtimeChoices,
                conditions.Select(c => c.GetRuntime(graphRuntime, dialogue)).ToList());
        }
    }
}
