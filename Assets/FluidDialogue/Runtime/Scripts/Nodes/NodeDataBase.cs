using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public interface INodeData : IGetRuntime<INode> {
    }

    public abstract class NodeDataBase : ScriptableObject, INodeData {
        public abstract INode GetRuntime ();
    }
}
