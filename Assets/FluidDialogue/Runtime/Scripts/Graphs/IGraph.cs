using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Graphs {
    public interface IGraph {
        INode Root { get; }
        INode GetCopy (INodeData nodeData);
    }
}
