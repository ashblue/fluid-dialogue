using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Conditions {
    public interface ICondition : IUniqueId {
        IConditionData Data { get; }

        bool GetIsValid (INode parent);
    }
}
