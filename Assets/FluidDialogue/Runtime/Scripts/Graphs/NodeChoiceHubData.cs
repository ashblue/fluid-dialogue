using System.Linq;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    [CreateMenu("Hub/Choice")]
    public class NodeChoiceHubData : NodeDataChoiceBase {
        protected override string DefaultName => "Choice Hub";
        public override bool HideInspectorActions => true;

        public override INode GetRuntime (IDialogueController dialogue) {
            var runtimeChoices = choices.Select(c => c.GetRuntime(dialogue)).ToList();
            return new NodeChoiceHub(
                null,
                runtimeChoices,
                conditions.Select(c => c.GetRuntime(dialogue)).ToList());
        }
    }
}
