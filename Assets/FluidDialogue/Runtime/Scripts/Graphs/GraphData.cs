using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Graphs {
    public interface IGraphData {
        INodeData Root { get; }
    }

    public class GraphData : ScriptableObject, IGraphData {
        [SerializeField]
        private NodeRootData _root = null;

        public INodeData Root => _root;
    }
}
