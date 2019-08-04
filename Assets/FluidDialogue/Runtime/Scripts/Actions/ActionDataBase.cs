using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Actions {
    public abstract class ActionDataBase : NodeNestedDataBase<IAction>, IActionData {
        public virtual void OnInit (IDialogueController dialogue) {}

        public virtual void OnStart () {}

        public virtual ActionStatus OnUpdate () {
            return ActionStatus.Success;
        }

        public virtual void OnExit () {}

        public virtual void OnReset () {}

        public override IAction GetRuntime (IDialogueController dialogue) {
            return new ActionRuntime(dialogue, _uniqueId, Instantiate(this));
        }
    }
}
