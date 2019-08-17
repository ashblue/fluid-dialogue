using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Choices {
    public interface IChoice : IUniqueId {
        string Text { get; }
        bool IsValid { get; }

        INode GetValidChildNode ();
    }
}
