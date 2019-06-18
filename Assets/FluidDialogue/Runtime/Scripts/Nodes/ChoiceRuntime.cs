namespace CleverCrow.Fluid.Dialogues.Nodes {
    public interface IChoiceRuntime {
        INodeRuntime Node { get; }
    }

    public class ChoiceRuntime : IChoiceRuntime {
        public INodeRuntime Node { get; }
    }
}
