namespace CleverCrow.Fluid.Dialogues.Graphs {
    public interface IDialogueNode {
        string Dialogue { get; }
        IActor Actor { get; }

        IDialogueNode Next ();
    }
}
