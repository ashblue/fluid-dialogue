namespace CleverCrow.Fluid.Dialogues.Builders {
    public static class A {
        public static DialogueGraphStubBuilder Graph () {
            return new DialogueGraphStubBuilder();
        }

        public static DialogueNodeStubBuilder Node () {
            return new DialogueNodeStubBuilder();
        }
    }
}
