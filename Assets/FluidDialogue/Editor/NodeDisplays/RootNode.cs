using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    [NodeType(typeof(NodeRootData))]
    public class RootNode : NodeDisplayBase {
        private const float PADDING_HEADER = 5;
        private const float PADDING_CONTENT = 20;
        private const float HEADER_HEIGHT = 20;

        private NodeBoxStyle _contentStyle;
        private NodeBoxStyle _headerStyle;
        private NodeBoxStyle _containerHighlight;
        private HeaderTextStyle _headerTextStyle;

        protected override void OnSetup () {
            var bodyColor = Color.gray;
            _contentStyle = new NodeBoxStyle(bodyColor, bodyColor);
            _containerHighlight = new NodeBoxStyle(Color.white, Color.clear);
            _headerTextStyle = new HeaderTextStyle();

            var headerColor = bodyColor * 1.3f;
            headerColor.a = 1f;
            _headerStyle = new NodeBoxStyle(headerColor, headerColor);

            Data.rect.width = 100;
            Data.rect.height = 100;
        }

        public override void Print () {
            PrintHeader();
            PrintBody();

            if (IsSelected) {
                GUI.Box(Data.rect, GUIContent.none, _containerHighlight.Style);
            }
        }

        private void PrintHeader () {
            var headerBox = new Rect(Data.rect.x, Data.rect.y, Data.rect.width, HEADER_HEIGHT);
            var headerArea = new Rect(
                headerBox.x + PADDING_HEADER / 2f,
                headerBox.y + PADDING_HEADER / 2f,
                headerBox.width - PADDING_HEADER,
                headerBox.height - PADDING_HEADER);

            GUI.Box(headerBox, GUIContent.none, _headerStyle.Style);
            GUI.Label(headerArea, Data.name, _headerTextStyle.Style);
        }

        private void PrintBody () {
            var box = new Rect(Data.rect.x, Data.rect.y + HEADER_HEIGHT, Data.rect.width, Data.rect.height - HEADER_HEIGHT);
            var content = new Rect(
                Data.rect.x + PADDING_CONTENT / 2f,
                Data.rect.y + HEADER_HEIGHT + PADDING_CONTENT / 2f,
                Data.rect.width - PADDING_CONTENT,
                Data.rect.height - PADDING_CONTENT);

            GUI.Box(box, GUIContent.none, _contentStyle.Style);

            GUILayout.BeginArea(content);
            GUILayout.Label(Data.name);
            GUILayout.EndArea();
        }
    }
}
