using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Conditions {
    public abstract class ConditionDataBase : NodeNestedDataBase<ICondition> {
        protected abstract bool OnGetIsValid ();
        protected virtual void OnInit (IDialogueController dialogue) {}

        public override ICondition GetRuntime (IDialogueController dialogue) {
            var copy = Instantiate(this);
            return new ConditionRuntime (dialogue, _uniqueId) {
                OnGetIsValid = copy.OnGetIsValid,
                OnInit = copy.OnInit,
            };
        }
    }
}
