using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Conditions;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public interface INodeData : IGetRuntime<INode> {
    }

    public abstract class NodeDataBase : ScriptableObject, INodeData {
        public List<NodeDataBase> children;
        public List<ConditionDataBase> conditions;
        public List<ActionDataBase> enterActions;
        public List<ActionDataBase> exitActions;

        public abstract INode GetRuntime ();
    }
}
