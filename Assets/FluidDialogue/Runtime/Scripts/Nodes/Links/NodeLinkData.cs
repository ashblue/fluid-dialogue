using System.Linq;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeLinkData : NodeDataBase {
        public override INode GetRuntime (IDialogueController dialogue) {
            return new NodeLink(
                UniqueId,
                children.Count > 0 ? children[0].GetRuntime(dialogue) : null,
                conditions.Select(c => c.GetRuntime(dialogue)).ToList(),
                enterActions.Select(c => c.GetRuntime(dialogue)).ToList(),
                exitActions.Select(c => c.GetRuntime(dialogue)).ToList()
            );
        }
    }
}
