using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    [NodeType(typeof(NodeRootData))]
    public class RootNode : NodeDisplayBase {
        private const float PADDING = 20;

        private NodeBoxStyle _contentStyle;
        private NodeBoxStyle _contentHighlightStyle;

        private NodeBoxStyle CurrentStyle => IsSelected ?
            _contentHighlightStyle : _contentStyle;

        protected override void OnSetup () {
            _contentStyle = new NodeBoxStyle(Color.gray, Color.gray);
            _contentHighlightStyle = new NodeBoxStyle(Color.white, Color.gray);

            Data.rect.width = 100;
            Data.rect.height = 40;
        }

        public override void Print () {
            var contentArea = new Rect(
                Data.rect.x + PADDING / 2f,
                Data.rect.y + PADDING / 2f,
                Data.rect.width - PADDING,
                Data.rect.height - PADDING);

            GUI.Box(Data.rect, GUIContent.none, CurrentStyle.Style);

            GUILayout.BeginArea(contentArea);
            GUILayout.Label(Data.name);
            GUILayout.EndArea();
        }
    }
}
