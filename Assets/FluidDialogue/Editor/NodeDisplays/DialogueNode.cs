using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    [NodeType(typeof(NodeDialogueData))]
    public class DialogueNode : NodeDisplayBase {
        private NodeDialogueData _data;
        private Connection _in;

        protected override Color NodeColor { get; } = new Color(0.28f, 0.75f, 0.34f);
        protected override float NodeWidth { get; } = 200;
        protected override string NodeTitle => string.IsNullOrEmpty(_data.nodeTitle) ? _data.name : _data.nodeTitle;

        protected override void OnSetup () {
            _data = Data as NodeDialogueData;
            _in = CreateConnection(ConnectionType.In);
        }

        protected override void OnPrintBody () {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("actor"), GUIContent.none);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dialogue"), GUIContent.none);

            serializedObject.ApplyModifiedProperties();
        }

        protected override void OnUpdate () {
            var inPosition = Data.rect.position;
            inPosition.x -= Connection.Size / 2;
            inPosition.y += Data.rect.height / 2 - Connection.Size / 2;
            _in.SetPosition(inPosition);
        }

        public override void ShowContextMenu () {
            var menu = new GenericMenu();
            menu.AddItem(
                new GUIContent("Duplicate"), false, () => {
                    Window.DuplicateNode(this);
                });
            menu.AddItem(
                new GUIContent("Delete"), false, () => {
                    Window.DeleteNode(this);
                });
            menu.ShowAsContext();
        }
    }
}
