namespace CleverCrow.Fluid.Dialogues {
    public interface IDialoguePlaybackEvents {
        IUnityEvent Begin { get; }
        IUnityEvent End { get; }
        IUnityEvent<IActor, string> Speak { get; }
    }
}
