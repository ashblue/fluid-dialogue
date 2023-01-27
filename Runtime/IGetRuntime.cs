using CleverCrow.Fluid.Dialogues.Graphs;

namespace CleverCrow.Fluid.Dialogues {
    public interface IGetRuntime<T> : ISetup {
        T GetRuntime (IGraph graphRuntime, IDialogueController dialogue);
    }
}
