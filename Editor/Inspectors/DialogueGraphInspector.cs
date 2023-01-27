using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes;
using CleverCrow.Fluid.SimpleSpellcheck;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.Inspectors {
    [CustomEditor(typeof(DialogueGraph))]
    public class DialogueGraphInspector : Editor {
        private SerializedProperty _nodes;

        private void OnEnable () {
            _nodes = serializedObject.FindProperty("_nodes");
        }

        public override void OnInspectorGUI () {
            DrawDefaultInspector();

            if (GUILayout.Button("Spell Check")) {
                RunSpellCheck();
            }

            if (GUILayout.Button("Edit Dialogue")) {
                DialogueWindow.ShowGraph(target as DialogueGraph);
            }
        }

        private void RunSpellCheck () {
            var logList = new List<LogEntry>();

            for (var i = 0; i < _nodes.arraySize; i++) {
                var node = new SerializedObject(_nodes.GetArrayElementAtIndex(i).objectReferenceValue);
                CreateLog(node, logList);
            }

            SpellCheck.Instance.ShowLogs($"Dialogue: {target.name}", logList);
        }

        private void CreateLog (SerializedObject node, List<LogEntry> logList) {
            var textProp = node.FindProperty("dialogue");
            var choiceProp = node.FindProperty("choices");

            if (textProp == null && choiceProp == null) return;

            var textIsInvalid = textProp != null && SpellCheck.Instance.IsInvalid(textProp.stringValue);

            var choiceIsInvalid = false;
            if (choiceProp != null) {
                for (var j = 0; j < choiceProp.arraySize; j++) {
                    var choice = choiceProp.GetArrayElementAtIndex(j).objectReferenceValue as ChoiceData;
                    choiceIsInvalid = SpellCheck.Instance.IsInvalid(choice.text);
                    if (choiceIsInvalid) break;
                }
            }

            if (textIsInvalid || choiceIsInvalid) {
                var preview = "Invalid choice(s)";
                if (textProp != null) preview = textProp.stringValue;

                var log = new LogEntry(preview, () => {
                    NodeDataBaseEditor.ShowValidation(node.targetObject as NodeDataBase);
                    Selection.activeObject = node.targetObject;
                });

                logList.Add(log);
            }
        }
    }
}
