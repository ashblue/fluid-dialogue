using System;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Conditions {
    public abstract class ConditionDataBase : ScriptableObject, IGetRuntime<ICondition> {
        [SerializeField]
        private string _uniqueId;

        public string UniqueId => _uniqueId;

        public void Setup () {
            _uniqueId = Guid.NewGuid().ToString();
        }

        protected abstract bool OnGetIsValid ();
        protected virtual void OnInit (IDialogueController dialogue) {}

        public ICondition GetRuntime (IDialogueController dialogue) {
            return new ConditionRuntime (dialogue, _uniqueId) {
                OnGetIsValid = OnGetIsValid,
                OnInit = OnInit,
            };
        }
    }
}
