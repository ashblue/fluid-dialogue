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

        public ICondition GetRuntime () {
            return new ConditionRuntime (_uniqueId) {
                OnGetIsValid = OnGetIsValid,
            };
        }
    }
}
