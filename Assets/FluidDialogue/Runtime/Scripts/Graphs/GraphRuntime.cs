using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Graphs {
    public class GraphRuntime : IGraph {
        public INode Root { get; }

        public GraphRuntime (IDialogueController dialogue, IGraphData data) {
            Root = data.Root.GetRuntime(dialogue);
        }
    }
}
