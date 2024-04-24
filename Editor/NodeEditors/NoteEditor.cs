using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    [NodeType(typeof(NodeNoteData))]
    public class NoteEditor : NodeEditorBase {
        protected override Color NodeColor { get; } = Color.yellow;
        protected override float NodeWidth { get; } = 200;

        protected override void OnPrintBody (Event e) {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("note"), GUIContent.none);
            serializedObject.ApplyModifiedProperties();
        }

    }
}
