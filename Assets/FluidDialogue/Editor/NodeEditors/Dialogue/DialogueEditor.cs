using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    [NodeType(typeof(NodeDialogueData))]
    public class DialogueEditor : NodeEditorBase {
        private NodeDialogueData _data;
        private ChoiceCollection _choices;

        protected override Color NodeColor { get; } = new Color(0.28f, 0.75f, 0.34f);
        protected override float NodeWidth { get; } = 200;

        protected override void OnSetup () {
            _data = Data as NodeDialogueData;
            _choices = new ChoiceCollection(this, Window);
        }

        protected override void OnPrintBody (Event e) {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("actor"), GUIContent.none);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dialogue"), GUIContent.none);

            _choices.Print(e);
            CreateChoice();

            serializedObject.ApplyModifiedProperties();
        }

        private void CreateChoice () {
            if (GUILayout.Button("Add Choice", EditorStyles.miniButton, GUILayout.Width(80))) {
                _choices.Add();
            }
        }

        public override NodeDataBase CreateDataCopy () {
            return _choices.GetParentDataCopy();
        }

        protected override void OnDeleteCleanup () {
            _choices.DeleteAll();
        }
    }
}
