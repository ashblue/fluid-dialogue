using System.Linq;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes.PlayGraph;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodePlayGraphData : NodeDataBase {
        public DialogueGraph dialogueGraph;

        public override INode GetRuntime (IDialogueController dialogue) {
            return new NodePlayGraph(
                UniqueId,
                dialogueGraph,
                children.Select(c => c.GetRuntime(dialogue)).ToList(),
                conditions.Select(c => c.GetRuntime(dialogue)).ToList(),
                enterActions.Select(c => c.GetRuntime(dialogue)).ToList(),
                exitActions.Select(c => c.GetRuntime(dialogue)).ToList()
            );
        }
    }
}
