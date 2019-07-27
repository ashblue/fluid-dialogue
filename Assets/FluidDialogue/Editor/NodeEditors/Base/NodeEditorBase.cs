using System;
using System.Collections.Generic;
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
        protected Rect _contentArea;

        protected DialogueWindow Window { get; private set; }
        protected virtual string NodeTitle => Data.name;

        protected SerializedObject serializedObject { get; private set; }
        public NodeDataBase Data { get; private set; }
        private bool IsSelected { get; set; }

        protected virtual Color NodeColor => Color.gray;
        protected virtual float NodeWidth { get; } = 100;

        public bool IsMemoryLeak => Data == null;
        public virtual bool Protected => false;

        public void Setup (DialogueWindow window, NodeDataBase data) {
            Window = window;
            Data = data;
            _styles = new NodeStyles(NodeColor);

            Data.rect.width = NodeWidth;
            serializedObject = new SerializedObject(data);

            if (HasOutConnection) {
                CreateConnection(ConnectionType.Out, data);
            }

            if (HasInConnection) {
                CreateConnection(ConnectionType.In, data);
            }

            OnSetup();
        }

        protected virtual void OnSetup () {
        }

        public void Print () {
            PositionConnections();
            PrintHeader();
            PrintBody();

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
            _contentArea = new Rect(
                Data.rect.x + PADDING_CONTENT / 2f,
                Data.rect.y + HEADER_HEIGHT + PADDING_CONTENT / 2f,
                Data.rect.width - PADDING_CONTENT,
                Data.rect.height - PADDING_CONTENT);

            GUI.Box(box, GUIContent.none, _styles.ContentStyle.Style);

            GUILayout.BeginArea(_contentArea);

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

        public virtual void ShowContextMenu () {
        }

        public virtual NodeDataBase CreateDataCopy () {
            return Data.GetCopy();
        }

        public void DeleteCleanup () {
            OnDeleteCleanup();
        }

        protected virtual void OnDeleteCleanup () {
        }
    }
}
