using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Actions {
    public abstract class ActionDataBase : NodeNestedDataBase<IAction> {
        protected virtual void OnInit (IDialogueController dialogue) {}

        protected virtual void OnStart () {}

        protected virtual ActionStatus OnUpdate () {
            return ActionStatus.Success;
        }

        protected virtual void OnExit () {}

        protected virtual void OnReset () {}

        public override IAction GetRuntime (IDialogueController dialogue) {
            var copy = Instantiate(this);
            return new ActionRuntime(dialogue, _uniqueId) {
                OnInit = copy.OnInit,
                OnStart = copy.OnStart,
                OnUpdate = copy.OnUpdate,
                OnExit = copy.OnExit,
                OnReset = copy.OnReset,
            };
        }
    }
}
