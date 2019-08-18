using System;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class NodeTypeAttribute : Attribute {
        public Type Type { get; }

        public NodeTypeAttribute (Type type) {
            Type = type;
        }
    }
}
