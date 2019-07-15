using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    [NodeType(typeof(NodeRootData))]
    public class RootNode : NodeDisplayBase {
        private const float PADDING = 20;
        private const float SIZE = 100;

        private NodeBoxStyle _contentStyle;

        protected override void OnSetup () {
            _contentStyle = new NodeBoxStyle(Color.gray, Color.gray);
        }

        public override void Print () {
            var boxArea = new Rect(
                _data.position.x,
                _data.position.y,
                SIZE,
                SIZE);

            var contentArea = new Rect(
                boxArea.x + PADDING / 2f,
                boxArea.y + PADDING / 2f,
                boxArea.width - PADDING,
                boxArea.height - PADDING);

            GUI.Box(boxArea, GUIContent.none, _contentStyle.Style);

            GUILayout.BeginArea(contentArea);
            GUILayout.Label(_data.name);
            GUILayout.EndArea();
        }
    }
}
