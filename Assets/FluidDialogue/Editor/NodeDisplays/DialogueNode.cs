using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    [NodeType(typeof(NodeDialogueData))]
    public class DialogueNode : NodeDisplayBase {
        protected override Color NodeColor { get; } = new Color(0.28f, 0.75f, 0.34f);
        protected override Vector2 NodeSize { get; } = new Vector2(200, 200);

        protected override void OnPrintBody () {
            var data = Data as NodeDialogueData;
            data.actor = EditorGUILayout.ObjectField(data.actor, typeof(ActorDefinition), false) as ActorDefinition;
            data.dialogue = GUILayout.TextArea(data.dialogue, GUILayout.Height(40));
        }
    }
}
