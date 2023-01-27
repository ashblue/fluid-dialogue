using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Nodes;
using CleverCrow.Fluid.SimpleSpellcheck;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.Inspectors {
    [CustomEditor(typeof(NodeDataBase), true)]
    public class NodeDataBaseEditor : Editor {
        private SerializedProperty _dialogue;
        private SerializedProperty _choices;

        private ConditionSortableList _conditions;
        private ActionsSortableList _enterActions;
        private ActionsSortableList _exitActions;

        private void OnEnable () {
            var node = target as NodeDataBase;

            _dialogue = serializedObject.FindProperty("dialogue");
            _choices = serializedObject.FindProperty("choices");

            _conditions = new ConditionSortableList(this, "conditions", node, node.conditions);
            if (!node.HideInspectorActions) {
                _enterActions = new ActionsSortableList(this, "enterActions", node, node.enterActions);
                _exitActions = new ActionsSortableList(this, "exitActions", node, node.exitActions);
            }
        }

        public override void OnInspectorGUI () {
            base.OnInspectorGUI();
            SpellCheckText();

            _conditions.Update();
            _enterActions?.Update();
            _exitActions?.Update();
        }

        private void SpellCheckText () {
            if (_dialogue == null && _choices == null) return;
            if (!GUILayout.Button("Spell Check")) return;

            ShowValidation(target as NodeDataBase);
        }

        public static void ShowValidation (NodeDataBase target) {
            var serializedObject = new SerializedObject(target);
            var dialogue = serializedObject.FindProperty("dialogue");
            var choices = serializedObject.FindProperty("choices");

            SpellCheck.Instance.ClearValidation();

            if (dialogue != null) {
                SpellCheck.Instance.AddValidation(dialogue.displayName, dialogue.stringValue);
            }

            if (choices != null) {
                for (var i = 0; i < choices.arraySize; i++) {
                    var choice = choices.GetArrayElementAtIndex(i).objectReferenceValue as ChoiceData;
                    SpellCheck.Instance.AddValidation($"Choice {i}", choice.text);
                }
            }
        }
    }
}
