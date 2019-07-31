using System;

namespace CleverCrow.Fluid.Dialogues.Actions {
    public class CreateActionMenuAttribute : Attribute {
        public string Path { get; }
        public int Priority { get; }

        public CreateActionMenuAttribute (string path, int priority = 0) {
            Path = path;
            Priority = priority;
        }
    }
}
