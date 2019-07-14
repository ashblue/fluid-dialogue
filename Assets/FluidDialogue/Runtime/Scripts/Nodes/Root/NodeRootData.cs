using System.Linq;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeRootData : NodeDataBase {
        public override INode GetRuntime (IDialogueController dialogue) {
            return new NodeRoot(
                UniqueId,
                children.Select(c => c.GetRuntime(dialogue)).ToList(),
                conditions.Select(c => c.GetRuntime(dialogue)).ToList(),
                enterActions.Select(c => c.GetRuntime(dialogue)).ToList(),
                exitActions.Select(c => c.GetRuntime(dialogue)).ToList()
            );
        }
    }
}
