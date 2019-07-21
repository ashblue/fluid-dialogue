using System;
using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    public abstract class NodeDisplayBase {
        private const float PADDING_HEADER = 5;
        private const float PADDING_CONTENT = 20;
        private const float HEADER_HEIGHT = 20;

        private readonly List<Connection> _connections = new List<Connection>();

        private NodeBoxStyle _contentStyle;
        private NodeBoxStyle _headerStyle;
        private NodeBoxStyle _containerHighlight;
        private HeaderTextStyle _headerTextStyle;
        private float _cachedContentHeight;
        private bool _dragInit;
        private bool _isDragging;

        protected DialogueWindow Window { get; private set; }
        protected virtual string NodeTitle => Data.name;
        protected SerializedObject serializedObject { get; private set; }
        public NodeDataBase Data { get; private set; }
        public bool IsSelected { get; private set; }

        protected virtual Color NodeColor => Color.gray;
        protected virtual float NodeWidth { get; } = 100;

        public bool IsMemoryLeak => Data == null;
        public virtual bool Protected => false;

        public void Setup (DialogueWindow window, NodeDataBase data) {
            Window = window;
            Data = data;

            var bodyColor = NodeColor;
            _contentStyle = new NodeBoxStyle(bodyColor, bodyColor);
            _containerHighlight = new NodeBoxStyle(Color.white, Color.clear);
            _headerTextStyle = new HeaderTextStyle();

            var headerColor = bodyColor * 1.3f;
            headerColor.a = 1f;
            _headerStyle = new NodeBoxStyle(headerColor, headerColor);

            Data.rect.width = NodeWidth;
            serializedObject = new SerializedObject(data);

            OnSetup();
        }

        protected virtual void OnSetup () {
        }

        public void Print () {
            Update();
            PrintHeader();
            PrintBody();

            if (IsSelected) {
                GUI.Box(Data.rect, GUIContent.none, _containerHighlight.Style);
            }

            PrintConnections();
        }

        private void PrintConnections () {
            foreach (var connection in _connections) {
                connection.Print();
            }
        }

        private void PrintHeader () {
            var headerBox = new Rect(Data.rect.x, Data.rect.y, Data.rect.width, HEADER_HEIGHT);
            var headerArea = new Rect(
                headerBox.x + PADDING_HEADER / 2f,
                headerBox.y + PADDING_HEADER / 2f,
                headerBox.width - PADDING_HEADER,
                headerBox.height - PADDING_HEADER);

            GUI.Box(headerBox, GUIContent.none, _headerStyle.Style);
            GUI.Label(headerArea, NodeTitle, _headerTextStyle.Style);
        }

        private void PrintBody () {
            var box = new Rect(Data.rect.x, Data.rect.y + HEADER_HEIGHT, Data.rect.width, Data.rect.height - HEADER_HEIGHT);
            var content = new Rect(
                Data.rect.x + PADDING_CONTENT / 2f,
                Data.rect.y + HEADER_HEIGHT + PADDING_CONTENT / 2f,
                Data.rect.width - PADDING_CONTENT,
                Data.rect.height - PADDING_CONTENT);

            GUI.Box(box, GUIContent.none, _contentStyle.Style);

            GUILayout.BeginArea(content);

            OnPrintBody();
            if (Event.current.type == EventType.Repaint ) {
                var rect = GUILayoutUtility.GetLastRect();
                if (Math.Abs(rect.height - _cachedContentHeight) > 0.1f) {
                    Data.rect.height = rect.height + HEADER_HEIGHT + PADDING_CONTENT * 2;
                    _cachedContentHeight = rect.height;
                }
            }

            GUILayout.EndArea();
        }

        protected virtual void OnPrintBody () {
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

        public Connection GetConnection (Vector2 mousePosition) {
            return _connections.Find(c => c.IsClicked(mousePosition));
        }

        protected Connection CreateConnection (ConnectionType type) {
            var connection = new Connection(type, Data);
            _connections.Add(connection);

            return connection;
        }

        private void Update () {
            OnUpdate();
        }

        protected virtual void OnUpdate () {
        }
    }
}
