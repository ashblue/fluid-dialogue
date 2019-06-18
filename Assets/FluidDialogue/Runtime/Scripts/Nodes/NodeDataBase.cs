using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public interface INodeData {
        INodeRuntime GetRuntime ();
    }

    public abstract class NodeDataBase : ScriptableObject, INodeData {
        public abstract INodeRuntime GetRuntime ();
    }
}
