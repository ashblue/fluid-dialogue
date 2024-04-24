using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    [NodeType(typeof(NodeDialogueData))]
    public class DialogueEditor : NodeEditorBase {
        private ChoiceCollection _choices;

        protected override Color NodeColor { get; } = new Color(0.28f, 0.75f, 0.34f);
        protected override float NodeWidth { get; } = 200;

        protected override void OnSetup () {
            _choices = new ChoiceCollection(this, Data as NodeDataChoiceBase, Window);
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
                GUI.FocusControl(null);
                _choices.Add();
            }
        }

        protected override NodeDataBase OnCreateDataCopy (NodeDataBase copy) {
            return _choices.GetParentDataCopy(copy as NodeDataChoiceBase);
        }

        protected override void OnDeleteCleanup () {
            _choices.DeleteAll();
        }
    }
}
