using System;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class CreateNodeMenuAttribute : Attribute {
        public string Path { get; }

        public CreateNodeMenuAttribute (string path) {
            Path = path;
        }
    }
}
