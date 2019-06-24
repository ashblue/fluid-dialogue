using System;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions {
    public abstract class ActionDataBase : ScriptableObject, IGetRuntime<IAction> {
        [SerializeField]
        private string _uniqueId;

        protected virtual void OnInit () {}

        protected virtual void OnStart () {}

        protected virtual ActionStatus OnUpdate () {
            return ActionStatus.Success;
        }

        protected virtual void OnExit () {}

        protected virtual void OnReset () {}

        public string UniqueId => _uniqueId;

        public void Setup () {
            _uniqueId = Guid.NewGuid().ToString();
        }

        public IAction GetRuntime () {
            return new ActionRuntime(_uniqueId) {
                OnInit = OnInit,
                OnStart = OnStart,
                OnUpdate = OnUpdate,
                OnExit = OnExit,
                OnReset = OnReset,
            };
        }
    }
}
