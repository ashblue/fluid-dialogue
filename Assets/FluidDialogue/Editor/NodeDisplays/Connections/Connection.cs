using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    public class Connection {
        private static Texture2D _graphic;

        private readonly List<Connection> _connections = new List<Connection>();
        private readonly ConnectionType _type;

        private bool _exampleCurveActive;
        private Vector2 _exampleCurveTarget;

        private Rect _rect = new Rect(Vector2.zero, new Vector2(Size, Size));

        private static Texture2D Graphic {
            get {
                if (_graphic == null) _graphic = Resources.Load<Texture2D>("dot");
                return _graphic;
            }
        }

        public static float Size { get; } = 16;

        public Connection (ConnectionType type) {
            _type = type;
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

        public void AddConnection (Connection target) {
            if (target == null
                || target._type == _type
                || _connections.Contains(target)) return;

            _connections.Add(target);
        }
    }
}
