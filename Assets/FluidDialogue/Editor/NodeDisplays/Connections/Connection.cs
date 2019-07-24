using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    public interface IConnection {
        ConnectionType Type { get; }
        Rect Rect { get; }
        INodeData Data { get; }

        void AddConnection (IConnection connection);
        void AddParent (IConnection parent);
        void RemoveParent (IConnection parent);
        void RemoveConnection (IConnection connection);
    }

    public class Connection : IConnection {
        public const float SIZE = 16;
        private static Texture2D _graphic;

        private readonly INodeData _data;
        private readonly List<IConnection> _connections = new List<IConnection>();
        private readonly ConnectionType _type;
        private readonly IDialogueWindow _window;
        private readonly List<IConnection> _parents = new List<IConnection>();

        private bool _exampleCurveActive;
        private Vector2 _exampleCurveTarget;

        private Rect _rect = new Rect(Vector2.zero, new Vector2(SIZE, SIZE));

        public ConnectionType Type => _type;
        public Rect Rect => _rect;
        public INodeData Data => _data;

        private static Texture2D Graphic {
            get {
                if (_graphic == null) _graphic = Resources.Load<Texture2D>("dot");
                return _graphic;
            }
        }

        private bool IsMemoryLeak => _data.Children.Count != _connections.Count;
        public IReadOnlyList<IConnection> Parents => _parents;

        public Connection (ConnectionType type, INodeData data, IDialogueWindow window) {
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
                PaintCurve(connection.Rect.center);
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

            ClearAllConnections();
            foreach (var child in _data.Children) {
                var target = _window.DataToNode[child];
                BindConnection(target.In);
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

        public void AddConnection (IConnection target) {
            if (target == null
                || target.Type == _type
                || _connections.Contains(target)) return;

            if (_type == ConnectionType.In) {
                target.AddConnection(this);
                return;
            }

            BindConnection(target);

            if (_data is Object data) {
                Undo.RecordObject(data, "Add connection");
            }

            _data.Children.Add(target.Data as NodeDataBase);
        }

        private void BindConnection (IConnection target) {
            _connections.Add(target);
            target.AddParent(this);
        }

        public void AddParent (IConnection parent) {
            _parents.Add(parent);
        }

        public void RemoveParent (IConnection parent) {
            _parents.Remove(parent);
        }

        public void RemoveConnection (IConnection connection) {
            _connections.Remove(connection);
            _data.Children.Remove(connection.Data as NodeDataBase);
        }

        public void ShowContextMenu () {
            if (_type == ConnectionType.In) return;

            var menu = new GenericMenu();
            menu.AddItem(
                new GUIContent("Clear Connections"), false, () => {
                    Undo.RecordObject(_data as Object, "Clear connections");
                    ClearAllConnections();
                    _data.Children.Clear();
                });
            menu.ShowAsContext();
        }

        private void ClearAllConnections () {
            _connections.ForEach(c => c?.RemoveParent(this));
            _connections.Clear();
        }
    }
}
