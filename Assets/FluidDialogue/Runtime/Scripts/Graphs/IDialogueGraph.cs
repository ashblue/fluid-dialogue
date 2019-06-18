using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Graphs {
    public interface IDialogueGraph : IGraphClone {
        IDialogueNode Root { get; }
    }
}
