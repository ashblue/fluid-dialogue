using System.Linq;
using CleverCrow.Fluid.Dialogues.Graphs;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    [CreateMenu("Hub/Default")]
    public class NodeHubData : NodeDataBase {
        protected override string DefaultName => "Hub";
        public override INode GetRuntime (IGraph graphRuntime, IDialogueController dialogue) {
            return new NodeHub(
                graphRuntime,
                UniqueId,
                children.ToList<INodeData>(),
                conditions.Select(c => c.GetRuntime(graphRuntime, dialogue)).ToList(),
                enterActions.Select(c => c.GetRuntime(graphRuntime, dialogue)).ToList(),
                exitActions.Select(c => c.GetRuntime(graphRuntime, dialogue)).ToList()
            );
        }
    }
}
