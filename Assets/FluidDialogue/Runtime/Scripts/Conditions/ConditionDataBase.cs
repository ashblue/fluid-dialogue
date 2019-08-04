using System;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Conditions {
    public abstract class ConditionDataBase : ScriptableObject, IGetRuntime<ICondition> {
        [SerializeField]
        private string _title;

        [SerializeField]
        private string _uniqueId;

        public string UniqueId => _uniqueId;

        public void Setup () {
            if (string.IsNullOrEmpty(_title)) {
                _title = GetType().Name;
            }

            name = GetType().Name;
            _uniqueId = Guid.NewGuid().ToString();
        }

        protected abstract bool OnGetIsValid ();
        protected virtual void OnInit (IDialogueController dialogue) {}

        public ICondition GetRuntime (IDialogueController dialogue) {
            var copy = Instantiate(this);
            return new ConditionRuntime (dialogue, _uniqueId) {
                OnGetIsValid = copy.OnGetIsValid,
                OnInit = copy.OnInit,
            };
        }
    }
}
