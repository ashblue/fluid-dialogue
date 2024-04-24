using System;
using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Choices {
    public class ChoiceData : ScriptableObject, IGetRuntime<IChoice>, IConnectionChildCollection {
        public string text;

        [HideInInspector]
        public List<NodeDataBase> children = new List<NodeDataBase>();

        [HideInInspector]
        [SerializeField]
        private string _uniqueId;

        public string UniqueId => _uniqueId;

        public IReadOnlyList<NodeDataBase> Children => children;

        public void Setup () {
            name = "Choice";
            _uniqueId = Guid.NewGuid().ToString();
        }

        public IChoice GetRuntime (IGraph graphRuntime, IDialogueController dialogue) {
            return new ChoiceRuntime(
                graphRuntime,
                text,
                _uniqueId,
                children.ToList<INodeData>());
        }

        public void AddConnectionChild (NodeDataBase child) {
            children.Add(child);
        }

        public void RemoveConnectionChild (NodeDataBase child) {
            children.Remove(child);
        }

        public void SortConnectionsByPosition () {
            children = children.OrderBy(i => i.rect.yMin).ToList();
        }

        public void ClearConnectionChildren () {
            children.Clear();
        }
    }
}
