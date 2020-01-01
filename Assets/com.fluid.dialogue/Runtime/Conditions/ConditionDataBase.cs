using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Conditions {
    public abstract class ConditionDataBase : NodeNestedDataBase<ICondition>, IConditionData {
        public virtual void OnInit (IDialogueController dialogue) {}
        public abstract bool OnGetIsValid (INode parent);

        public override ICondition GetRuntime (IGraph graphRuntime, IDialogueController dialogue) {
            return new ConditionRuntime(dialogue, _uniqueId, Instantiate(this));
        }
    }
}
