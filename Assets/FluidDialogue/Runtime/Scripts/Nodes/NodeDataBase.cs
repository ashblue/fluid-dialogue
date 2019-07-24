using System;
using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Conditions;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public interface INodeData : IGetRuntime<INode> {
        List<NodeDataBase> Children { get; }
    }

    public abstract class NodeDataBase : ScriptableObject, INodeData {
        [HideInInspector]
        [SerializeField]
        private string _uniqueId;

        public List<NodeDataBase> children = new List<NodeDataBase>();
        public List<ConditionDataBase> conditions;
        public List<ActionDataBase> enterActions;
        public List<ActionDataBase> exitActions;
        public Rect rect;

        public string UniqueId => _uniqueId;
        public virtual string DefaultName { get; } = "Untitled";
        public List<NodeDataBase> Children => children;

        public void Setup () {
            _uniqueId = Guid.NewGuid().ToString();
        }

        public abstract INode GetRuntime (IDialogueController dialogue);
    }
}
