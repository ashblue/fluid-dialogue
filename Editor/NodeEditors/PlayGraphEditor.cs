using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    [NodeType(typeof(NodePlayGraphData))]
    public class PlayGraphEditor : NodeEditorBase {
        protected override Color NodeColor { get; } = new Color(0.75f, 0.52f, 0f);
        protected override float NodeWidth { get; } = 200;

        protected override void OnPrintBody (Event e) {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dialogueGraph"), GUIContent.none);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
