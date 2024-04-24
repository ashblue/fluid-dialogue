using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Graphs {
    public interface IGraph {
        INode Root { get; }
        IGraphData Data { get; }

        INode GetCopy (INodeData nodeData);
        INode GetNodeByDataId (string id);
    }
}
