using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    public class NodeStyles {
        public NodeBoxStyle ContentStyle { get; }
        public NodeBoxStyle HeaderStyle { get; }
        public NodeBoxStyle ContainerHighlightStyle { get; }
        public HeaderTextStyle HeaderTextStyle { get; }

        public NodeStyles (Color bodyColor) {
            ContentStyle = new NodeBoxStyle(bodyColor, bodyColor);
            ContainerHighlightStyle = new NodeBoxStyle(Color.white, Color.clear);
            HeaderTextStyle = new HeaderTextStyle();

            var headerColor = bodyColor * 1.3f;
            headerColor.a = 1f;
            HeaderStyle = new NodeBoxStyle(headerColor, headerColor);
        }
    }
}
