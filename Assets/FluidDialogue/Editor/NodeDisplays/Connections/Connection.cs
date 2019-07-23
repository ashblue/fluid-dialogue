using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    public class Connection {
        public const float SIZE = 16;
        private static Texture2D _graphic;

        private readonly NodeDataBase _data;
        private readonly List<Connection> _connections = new List<Connection>();
        private readonly ConnectionType _type;
        private readonly DialogueWindow _window;

        private bool _exampleCurveActive;
        private Vector2 _exampleCurveTarget;

        private Rect _rect = new Rect(Vector2.zero, new Vector2(SIZE, SIZE));

        private static Texture2D Graphic {
            get {
                if (_graphic == null) _graphic = Resources.Load<Texture2D>("dot");
                return _graphic;
            }
        }

        private bool IsMemoryLeak => _data.children.Count != _connections.Count;

        public Connection (ConnectionType type, NodeDataBase data, DialogueWindow window) {
            _window = window;
            _type = type;
            _data = data;
        }

        public void Print () {
            if (IsMemoryLeak) {
                RebuildConnections();
            }

            var color = GUI.color;
            GUI.color = Color.cyan;

            GUI.DrawTexture(_rect, Graphic);

            foreach (var connection in _connections) {
                PaintCurve(connection._rect.center);
            }

            if (_exampleCurveActive) {
                PaintCurve(_exampleCurveTarget);
            }

            GUI.color = color;
        }

        public void RebuildConnections () {
            if (_type == ConnectionType.In) {
                return;
            }

            _connections.Clear();
            foreach (var child in _data.children) {
                var target = _window.DataToNode[child];
                _connections.Add(target.In);
            }
        }

        private void PaintCurve (Vector2 destination) {
            Handles.DrawBezier(
                _rect.center,
                destination,
                _rect.center - Vector2.left * 50f,
                destination + Vector2.left * 50f,
                Color.cyan,
                null,
                2f
            );
            GUI.changed = true;
        }

        public bool IsClicked (Vector2 mousePosition) {
            return _rect.Contains(mousePosition);
        }

        public void SetPosition (Vector2 position) {
            _rect.position = position;
        }

        public void SetExampleCurve (Vector2 position) {
            _exampleCurveActive = true;
            _exampleCurveTarget = position;
        }

        public void ClearCurveExample () {
            _exampleCurveActive = false;
        }

        public void AddConnection (Connection target) {
            if (target == null
                || target._type == _type
                || _connections.Contains(target)) return;

            if (_type == ConnectionType.In) {
                target.AddConnection(this);
                return;
            }

            _connections.Add(target);

            Undo.RecordObject(_data, "Add connection");
            _data.children.Add(target._data);
        }

        public void ShowContextMenu () {
            if (_type == ConnectionType.In) return;

            var menu = new GenericMenu();
            menu.AddItem(
                new GUIContent("Clear Connections"), false, () => {
                    Undo.RecordObject(_data, "Clear connections");
                    _connections.Clear();
                    _data.children.Clear();
                });
            menu.ShowAsContext();
        }
    }
}
