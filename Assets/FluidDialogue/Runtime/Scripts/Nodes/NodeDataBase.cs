using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public interface INodeData {
        INode GetRuntime ();
    }

    public abstract class NodeDataBase : ScriptableObject, INodeData {
        public abstract INode GetRuntime ();
    }
}
