using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Graphs {
    public interface IGraphRuntime {
        INodeRuntime Root { get; }
    }

    public class GraphRuntime : IGraphRuntime {
        public INodeRuntime Root { get; }

        public GraphRuntime (IGraphData data) {
            Root = data.Root.GetRuntime();
        }
    }
}
