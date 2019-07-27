using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    [NodeType(typeof(NodeDialogueData))]
    public class DialogueEditor : NodeEditorBase {
        private NodeDialogueData _data;

        protected override Color NodeColor { get; } = new Color(0.28f, 0.75f, 0.34f);
        protected override float NodeWidth { get; } = 200;
        protected override string NodeTitle => string.IsNullOrEmpty(_data.nodeTitle) ? _data.name : _data.nodeTitle;

        private readonly List<Connection> _choiceConnections = new List<Connection>();

        protected override void OnSetup () {
            _data = Data as NodeDialogueData;

            foreach (var choice in _data.choices) {
                AddConnectionDisplay(choice);
            }
        }

        private void AddConnectionDisplay (ChoiceData choice) {
            Out[0].Hide = true;
            var connection = CreateConnection(ConnectionType.Out, choice);
            Out.Add(connection);
            _choiceConnections.Add(connection);
        }

        protected override void OnPrintBody (Event e) {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("actor"), GUIContent.none);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dialogue"), GUIContent.none);


            PrintChoices(e);
            CreateChoice();

            serializedObject.ApplyModifiedProperties();
        }

        private void CreateChoice () {
            if (GUILayout.Button("Add Choice", EditorStyles.miniButton, GUILayout.Width(80))) {
                var choice = ScriptableObject.CreateInstance<ChoiceData>();
                choice.name = "Choice";
                choice.Setup();
                _data.choices.Add(choice);

                AssetDatabase.AddObjectToAsset(choice, _data);
                AssetDatabase.SaveAssets();

                AddConnectionDisplay(choice);
            }
        }

        private void PrintChoices (Event e) {
            for (var i = 0; i < _data.choices.Count; i++) {
                GUILayout.BeginHorizontal();

                var choice = _data.choices[i];
                var connection = _choiceConnections[i];

                if (GUILayout.Button("Edit", EditorStyles.miniButton)) Selection.activeObject = choice;
                if (GUILayout.Button("-", EditorStyles.miniButton)) Debug.Log("Delete");
                choice.text = EditorGUILayout.TextField(choice.text);

                GUILayout.EndHorizontal();

                // Only draw on repaint events to prevent crashing display position
                if (e.type == EventType.Repaint) {
                    var area = GUILayoutUtility.GetLastRect();
                    var pos = _contentArea.position;
                    pos.x += _contentArea.width;
                    pos.y += area.y;
                    connection.SetPosition(pos);
                }
            }
        }

        public override void ShowContextMenu () {
            var menu = new GenericMenu();
            menu.AddItem(
                new GUIContent("Duplicate"), false, () => {
                    Window.Graph.DuplicateNode(this);
                });
            menu.AddItem(
                new GUIContent("Delete"), false, () => {
                    Window.Graph.DeleteNode(this);
                });
            menu.ShowAsContext();
        }
    }
}
