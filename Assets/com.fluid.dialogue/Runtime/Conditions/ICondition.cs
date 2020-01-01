using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Conditions {
    public interface ICondition : IUniqueId {
        bool GetIsValid (INode parent);
    }
}
