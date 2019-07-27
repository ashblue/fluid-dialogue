using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    [NodeType(typeof(NodeDialogueData))]
    public class DialogueEditor : NodeEditorBase {
        private readonly List<Connection> _choiceConnections = new List<Connection>();
        private readonly List<ChoiceData> _graveyard = new List<ChoiceData>();

        private NodeDialogueData _data;

        protected override Color NodeColor { get; } = new Color(0.28f, 0.75f, 0.34f);
        protected override float NodeWidth { get; } = 200;
        protected override string NodeTitle => string.IsNullOrEmpty(_data.nodeTitle) ? _data.name : _data.nodeTitle;

        private bool IsChoiceMemoryLeak => _data.choices.Count != _choiceConnections.Count;

        protected override void OnSetup () {
            _data = Data as NodeDialogueData;
            RebuildChoices();
        }

        private void AddConnectionDisplay (ChoiceData choice) {
            Out[0].Hide = true;
            CreateConnection(ConnectionType.Out, choice);
            _choiceConnections.Add(Out[Out.Count - 1]);
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
            if (IsChoiceMemoryLeak) {
                RebuildChoices();
            }

            for (var i = 0; i < _data.choices.Count; i++) {
                GUILayout.BeginHorizontal();

                var choice = _data.choices[i];
                var connection = _choiceConnections[i];

                if (GUILayout.Button("Edit", EditorStyles.miniButton)) Selection.activeObject = choice;
                if (GUILayout.Button("-", EditorStyles.miniButton)) _graveyard.Add(choice);
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

            if (_graveyard.Count > 0) {
                foreach (var choice in _graveyard) {
                    DeleteChoice(choice);
                }

                Out[0].Hide = _choiceConnections.Count != 0;
                _graveyard.Clear();
            }
        }


        private void RebuildChoices () {
            Out[0].Hide = false;
            _choiceConnections.ForEach(RemoveConnection);
            _choiceConnections.Clear();
            foreach (var choice in _data.choices) {
                AddConnectionDisplay(choice);
            }
        }

        private void DeleteChoice (ChoiceData choice) {
            var choiceIndex = _data.choices.IndexOf(choice);
            var connection = _choiceConnections[choiceIndex];

            Undo.SetCurrentGroupName($"Delete {choice.name}");
            Undo.RecordObject(Window.Graph, $"Delete {choice.name}");

            connection.Links.ClearAllLinks();
            _data.choices.Remove(choice);
            RemoveConnection(connection);
            _choiceConnections.Remove(connection);

            Undo.DestroyObjectImmediate(choice);
            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        public override void ShowContextMenu () {
            var menu = new GenericMenu();
            menu.AddItem(
                new GUIContent("Duplicate"), false, () => {
                    Window.GraphCrud.DuplicateNode(this);
                });
            menu.AddItem(
                new GUIContent("Delete"), false, () => {
                    Window.GraphCrud.DeleteNode(this);
                });
            menu.ShowAsContext();
        }
    }
}
