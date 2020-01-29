using System.Linq;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes.PlayGraph;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    [CreateMenu("Play Graph")]
    public class NodePlayGraphData : NodeDataBase {
        public DialogueGraph dialogueGraph;

        protected override string DefaultName => "Play Graph";

        public override INode GetRuntime (IGraph graphRuntime, IDialogueController dialogue) {
            return new NodePlayGraph(
                graphRuntime,
                UniqueId,
                dialogueGraph,
                children.ToList<INodeData>(),
                conditions.Select(c => c.GetRuntime(graphRuntime, dialogue)).ToList(),
                enterActions.Select(c => c.GetRuntime(graphRuntime, dialogue)).ToList(),
                exitActions.Select(c => c.GetRuntime(graphRuntime, dialogue)).ToList()
            );
        }
    }
}
