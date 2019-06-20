using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Choices {
    public interface IChoice {
        INode GetValidChildNode ();
    }
}
