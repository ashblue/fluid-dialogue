using System.Linq;
using CleverCrow.Fluid.Dialogues.Graphs;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeLinkData : NodeDataBase {
        public override INode GetRuntime (IGraph graphRuntime, IDialogueController dialogue) {
            return new NodeLink(
                graphRuntime,
                UniqueId,
                children.Count > 0 ? children[0] : null,
                conditions.Select(c => c.GetRuntime(graphRuntime, dialogue)).ToList(),
                enterActions.Select(c => c.GetRuntime(graphRuntime, dialogue)).ToList(),
                exitActions.Select(c => c.GetRuntime(graphRuntime, dialogue)).ToList()
            );
        }
    }
}
