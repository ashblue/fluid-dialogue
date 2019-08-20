using System;
using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    public abstract partial class NodeEditorBase {
        private const float PADDING_HEADER = 5;
        private const float PADDING_CONTENT = 20;
        private const float HEADER_HEIGHT = 20;

        private float _cachedContentHeight;
        private NodeStyles _styles;

        protected DialogueWindow Window { get; private set; }
        private string NodeTitle => string.IsNullOrEmpty(Data.nodeTitle) ? Data.name : Data.nodeTitle;

        protected SerializedObject serializedObject { get; private set; }
        public NodeDataBase Data { get; private set; }
        private bool IsSelected { get; set; }

        protected virtual Color NodeColor => Color.gray;
        protected virtual float NodeWidth { get; } = 100;

        public bool IsMemoryLeak => Data == null;
        public virtual bool Protected => false;
        public Rect ContentArea { get; private set; }

        public void Setup (DialogueWindow window, NodeDataBase data) {
            Window = window;
            Data = data;
            _styles = new NodeStyles(NodeColor);

            Data.rect.width = NodeWidth;
            serializedObject = new SerializedObject(data);

            CreateConnection(ConnectionType.Out, data, true);
            Out[0].Hide = !HasOutConnection;

            CreateConnection(ConnectionType.In, data, true);
            In[0].Hide = !HasInConnection;

            OnSetup();
        }

        protected virtual void OnSetup () {
        }

        public void Print () {
            PositionConnections();

            if (_cachedContentHeight < 1 || Window.MouseEvents.Scroll.ScrollRect.Overlaps(Data.rect)) {
                PrintHeader();
                PrintBody();
            }

            if (IsSelected) {
                GUI.Box(Data.rect, GUIContent.none, _styles.ContainerHighlightStyle.Style);
            }

            PrintConnections();
        }

        private void PrintHeader () {
            var headerBox = new Rect(Data.rect.x, Data.rect.y, Data.rect.width, HEADER_HEIGHT);
            var headerArea = new Rect(
                headerBox.x + PADDING_HEADER / 2f,
                headerBox.y + PADDING_HEADER / 2f,
                headerBox.width - PADDING_HEADER,
                headerBox.height - PADDING_HEADER);

            GUI.Box(headerBox, GUIContent.none, _styles.HeaderStyle.Style);
            GUI.Label(headerArea, NodeTitle, _styles.HeaderTextStyle.Style);
        }

        private void PrintBody () {
            var box = new Rect(Data.rect.x, Data.rect.y + HEADER_HEIGHT, Data.rect.width, Data.rect.height - HEADER_HEIGHT);
            ContentArea = new Rect(
                Data.rect.x + PADDING_CONTENT / 2f,
                Data.rect.y + HEADER_HEIGHT + PADDING_CONTENT / 2f,
                Data.rect.width - PADDING_CONTENT,
                Data.rect.height - PADDING_CONTENT);

            GUI.Box(box, GUIContent.none, _styles.ContentStyle.Style);

            GUILayout.BeginArea(ContentArea);

            GUILayout.BeginVertical();
            OnPrintBody(Event.current);
            GUILayout.EndVertical();

            if (Event.current.type == EventType.Repaint) {
                var rect = GUILayoutUtility.GetLastRect();
                if (Math.Abs(rect.height - _cachedContentHeight) > 0.1f) {
                    Data.rect.height = rect.height + PADDING_CONTENT * 2;
                    _cachedContentHeight = Data.rect.height;
                }
            }

            GUILayout.EndArea();
        }

        protected virtual void OnPrintBody (Event e) {
            GUILayout.Label(Data.name);
        }

        public void Select () {
            IsSelected = true;
        }

        public void Deselect () {
            IsSelected = false;
        }

        public void ShowContextMenu () {
            if (Protected) return;

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

        public NodeDataBase CreateDataCopy () {
            var copy = Data.GetDataCopy();

            var saveNeeded = new List<Object>();
            foreach (var action in copy.enterActions.Concat(copy.exitActions)) {
                action.Setup();
                saveNeeded.Add(action);
            }

            foreach (var condition in copy.conditions) {
                condition.Setup();
                saveNeeded.Add(condition);
            }

            foreach (var obj in saveNeeded) {
                if (FluidDialogueSettings.Current.HideNestedNodeData) {
                    obj.hideFlags = HideFlags.HideInHierarchy;
                }

                AssetDatabase.AddObjectToAsset(obj, Window.Graph);
                AssetDatabase.SaveAssets();
                Undo.RegisterCreatedObjectUndo(obj, "Duplicate object");
            }

            return OnCreateDataCopy(copy);
        }

        protected virtual NodeDataBase OnCreateDataCopy (NodeDataBase copy) {
            return copy;
        }

        public void DeleteCleanup () {
            OnDeleteCleanup();
        }

        protected virtual void OnDeleteCleanup () {
        }

        public bool IsHeaderPosition (Vector2 position) {
            var header = new Rect(Data.rect) {height = HEADER_HEIGHT};
            return header.Contains(position);
        }
    }
}
