namespace CleverCrow.Fluid.Dialogues.Builders {
    public static class A {
        public static DialogueGraphStubBuilder Graph => new DialogueGraphStubBuilder();
        public static DialogueNodeStubBuilder Node => new DialogueNodeStubBuilder();
        public static NodeDataStubBuilder NodeData => new NodeDataStubBuilder();
        public static ActionStubBuilder Action => new ActionStubBuilder();
        public static ChoiceStubBuilder Choice => new ChoiceStubBuilder();
    }
}
