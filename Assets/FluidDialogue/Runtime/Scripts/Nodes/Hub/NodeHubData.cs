using System.Linq;

namespace CleverCrow.Fluid.Dialogues.Nodes.Hub {
    public class NodeHubData : NodeDataBase {
        public override INode GetRuntime (IDialogueController dialogue) {
            return new NodeHub(
                UniqueId,
                children.Select(c => c.GetRuntime(dialogue)).ToList(),
                conditions.Select(c => c.GetRuntime(dialogue)).ToList(),
                enterActions.Select(c => c.GetRuntime(dialogue)).ToList(),
                exitActions.Select(c => c.GetRuntime(dialogue)).ToList()
            );
        }
    }
}
