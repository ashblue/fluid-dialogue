using System;
using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Conditions;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public interface INodeData : IGetRuntime<INode>, IConnectionChildCollection {
    }

    public interface IConnectionChildCollection {
        IReadOnlyList<NodeDataBase> Children { get; }

        void AddConnectionChild (NodeDataBase child);
        void RemoveConnectionChild (NodeDataBase child);
        void SortConnectionsByPosition ();
        void ClearConnectionChildren ();
    }

    public abstract class NodeDataBase : ScriptableObject, INodeData {
        [HideInInspector]
        [SerializeField]
        private string _uniqueId;

        [HideInInspector]
        public Rect rect;

        public string nodeTitle;

        [HideInInspector]
        public List<NodeDataBase> children = new List<NodeDataBase>();

        [HideInInspector]
        public List<ConditionDataBase> conditions = new List<ConditionDataBase>();

        [HideInInspector]
        public List<ActionDataBase> enterActions = new List<ActionDataBase>();

        [HideInInspector]
        public List<ActionDataBase> exitActions = new List<ActionDataBase>();

        public string UniqueId => _uniqueId;
        protected virtual string DefaultName { get; } = "Untitled";
        public IReadOnlyList<NodeDataBase> Children => children;

        public void Setup () {
            _uniqueId = Guid.NewGuid().ToString();
            name = DefaultName;
        }

        public void AddConnectionChild (NodeDataBase child) {
            children.Add(child);
        }

        public void RemoveConnectionChild (NodeDataBase child) {
            children.Remove(child);
        }

        public virtual void SortConnectionsByPosition () {
            children = children.OrderBy(i => i.rect.yMin).ToList();
        }

        public abstract INode GetRuntime (IDialogueController dialogue);

        public virtual void ClearConnectionChildren () {
            children.Clear();
        }

        public virtual NodeDataBase GetCopy () {
            return Instantiate(this);
        }
    }
}
