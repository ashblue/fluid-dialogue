using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions {
    public abstract class ActionDataBase : ScriptableObject {
        protected virtual void OnInit () {}

        protected virtual void OnStart () {}

        protected virtual ActionStatus OnUpdate () {
            return ActionStatus.Success;
        }

        protected virtual void OnExit () {}

        protected virtual void OnReset () {}

        public IAction CreateRuntime () {
            return new ActionRuntime {
                OnInit = OnInit,
                OnStart = OnStart,
                OnUpdate = OnUpdate,
                OnExit = OnExit,
                OnReset = OnReset,
            };
        }
    }
}
