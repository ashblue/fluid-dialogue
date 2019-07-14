using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Graphs {
    public interface IGraphData {
        INodeData Root { get; }
    }

    public class DialogueGraph : ScriptableObject, IGraphData {
        [HideInInspector]
        public NodeRootData root;

        public INodeData Root => root;
    }
}
