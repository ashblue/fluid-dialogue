using System;
using CleverCrow.Fluid.Dialogues.Graphs;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public abstract class NodeNestedDataBase<T> : ScriptableObject, IGetRuntime<T> {
        [SerializeField]
        private string _title;

        [SerializeField]
        protected string _uniqueId;

        public string UniqueId => _uniqueId;

        public void Setup () {
            if (string.IsNullOrEmpty(_title)) {
                _title = GetType().Name;
            }

            name = GetType().Name;
            _uniqueId = Guid.NewGuid().ToString();
        }

        public abstract T GetRuntime (IGraph graphRuntime, IDialogueController dialogue);
    }
}
