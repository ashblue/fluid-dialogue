using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Graphs {
    public interface IGraphData {
        INodeData Root { get; }
    }

    public class GraphData : ScriptableObject, IGraphData {
        [SerializeField]
        private NodeDataBase _root;

        public INodeData Root => _root;
    }
}
