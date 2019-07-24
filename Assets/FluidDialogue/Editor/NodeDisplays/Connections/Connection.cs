using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    public partial interface IConnection {
        ConnectionType Type { get; }
        Rect Rect { get; }
        INodeData Data { get; }
    }

    public partial class Connection : IConnection {
        public const float SIZE = 16;
        private static Texture2D _graphic;

        private readonly INodeData _data;
        private readonly ConnectionType _type;
        private readonly IDialogueWindow _window;

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
    }
}
