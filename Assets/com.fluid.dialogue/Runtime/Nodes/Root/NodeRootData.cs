using System.Linq;
using CleverCrow.Fluid.Dialogues.Graphs;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeRootData : NodeDataBase {
        protected override string DefaultName => "Root";

        public override INode GetRuntime (IGraph graphRuntime, IDialogueController dialogue) {
            return new NodeRoot(
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
