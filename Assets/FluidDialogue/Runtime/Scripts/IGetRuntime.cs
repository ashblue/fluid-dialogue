namespace CleverCrow.Fluid.Dialogues {
    public interface IGetRuntime<T> : ISetup {
        T GetRuntime (IDialogueController dialogue);
    }
}
