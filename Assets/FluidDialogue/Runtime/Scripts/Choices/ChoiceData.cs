using System;
using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Choices {
    public class ChoiceData : ScriptableObject, IGetRuntime<IChoice>, IConnectionChildCollection {
        public string text;
        public List<NodeDataBase> children = new List<NodeDataBase>();

        [SerializeField]
        private string _uniqueId;

        public string UniqueId => _uniqueId;

        public IReadOnlyList<NodeDataBase> Children => children;

        public void Setup () {
            _uniqueId = Guid.NewGuid().ToString();
        }

        public IChoice GetRuntime (IDialogueController dialogue) {
            return new ChoiceRuntime(
                _uniqueId,
                children.Select(c => c.GetRuntime(dialogue)).ToList());
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
