using System;

namespace CleverCrow.Fluid.Dialogues.Conditions {
    public class ConditionRuntime : ICondition {
        public Func<bool> OnGetIsValid { private get; set; }
        public string UniqueId { get; }

        public ConditionRuntime (string uniqueId) {
            UniqueId = uniqueId;
        }

        public bool GetIsValid () {
            return OnGetIsValid.Invoke();
        }
    }
}
