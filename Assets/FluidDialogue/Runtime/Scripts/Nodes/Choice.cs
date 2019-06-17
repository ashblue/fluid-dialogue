namespace CleverCrow.Fluid.Dialogues.Nodes {
    [System.Serializable]
    public class Choice : IChoice {
        public DialogueNodeBase node;

        public IDialogueNode Node => node;
    }
}
