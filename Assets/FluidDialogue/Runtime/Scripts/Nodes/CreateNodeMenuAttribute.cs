using System;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class CreateNodeMenuAttribute : Attribute {
        public string Path { get; }
        public int Priority { get; }

        public CreateNodeMenuAttribute (string path, int priority = 0) {
            Path = path;
            Priority = priority;
        }
    }
}
