using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    public class NodeStyles {
        public NodeBoxStyle ContentStyle { get; }
        public NodeBoxStyle HeaderStyle { get; }
        public NodeBoxStyle ContainerHighlightStyle { get; }
        public HeaderTextStyle HeaderTextStyle { get; }
        public Color BodyColor { get; }

        public NodeStyles (Color bodyColor) {
            BodyColor = bodyColor;

            ContentStyle = new NodeBoxStyle(BodyColor, BodyColor);
            var brightBodyColor = BodyColor * 1.3f;
            ContainerHighlightStyle = new NodeBoxStyle(Color.black, brightBodyColor);
            HeaderTextStyle = new HeaderTextStyle();

            var headerColor = BodyColor * 1.3f;
            headerColor.a = 1f;
            HeaderStyle = new NodeBoxStyle(headerColor, headerColor);
        }
    }
}
