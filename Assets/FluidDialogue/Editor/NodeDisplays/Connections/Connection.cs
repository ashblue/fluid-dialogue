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

        private bool _exampleCurveActive;
        private Vector2 _exampleCurveTarget;

        private Rect _rect = new Rect(Vector2.zero, new Vector2(SIZE, SIZE));

        private static Texture2D Graphic {
            get {
                if (_graphic == null) _graphic = Resources.Load<Texture2D>("dot");
                return _graphic;
            }
        }

        public UnityEventPlus<NodeDataBase> EventAddConnection { get; } = new UnityEventPlus<NodeDataBase>();
        public UnityEventPlus EventClearConnections { get; } = new UnityEventPlus();

        public Connection (ConnectionType type, NodeDataBase data) {
            _type = type;
            _data = data;
        }

        public void Print () {
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

        public void AddConnection (Connection target, bool skipEvents = false) {
            if (target == null
                || target._type == _type
                || _connections.Contains(target)) return;

            if (_type == ConnectionType.In) {
                target.AddConnection(this);
                return;
            }

            _connections.Add(target);

            if (!skipEvents) {
                EventAddConnection.Invoke(target._data);
            }
        }

        public void ShowContextMenu () {
            if (_type == ConnectionType.In) return;

            var menu = new GenericMenu();
            menu.AddItem(
                new GUIContent("Clear Connections"), false, () => {
                    _connections.Clear();
                    EventClearConnections.Invoke();
                });
            menu.ShowAsContext();
        }
    }
}
