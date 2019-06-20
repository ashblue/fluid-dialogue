using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Conditions {
    public abstract class ConditionDataBase : ScriptableObject, IGetRuntime<ICondition> {
        protected abstract bool OnGetIsValid ();

        public ICondition GetRuntime () {
            return new ConditionRuntime {
                OnGetIsValid = OnGetIsValid,
            };
        }
    }
}
