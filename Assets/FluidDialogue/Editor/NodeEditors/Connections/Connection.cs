using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    public partial interface IConnection {
        ConnectionType Type { get; }
        Rect Rect { get; }
        INodeData Data { get; }
        IConnectionLinks Links { get; }
    }

    public partial class Connection : IConnection {
        public const float SIZE = 16;
        private static Texture2D _graphic;

        private Rect _rect = new Rect(Vector2.zero, new Vector2(SIZE, SIZE));

        public ConnectionType Type { get; }
        public Rect Rect => _rect;
        public INodeData Data { get; }
        public IConnectionLinks Links { get; }
        public IDialogueWindow Window { get; }

        private static Texture2D Graphic {
            get {
                if (_graphic == null) _graphic = Resources.Load<Texture2D>("dot");
                return _graphic;
            }
        }

        private bool IsMemoryLeak => Data.Children.Count != Links.List.Count;

        public Connection (ConnectionType type, INodeData data, IDialogueWindow window) {
            Window = window;
            Type = type;
            Data = data;
            Links = new ConnectionLinks(this);
        }

        public void Print () {
            if (IsMemoryLeak) {
                Links.RebuildLinks();
            }

            var color = GUI.color;
            GUI.color = Color.cyan;

            GUI.DrawTexture(_rect, Graphic);

            foreach (var connection in Links.List) {
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

        public void ShowContextMenu () {
            if (Type == ConnectionType.In) return;

            var menu = new GenericMenu();
            menu.AddItem(
                new GUIContent("Clear Connections"), false, () => {
                    Undo.RecordObject(Data as Object, "Clear connections");
                    Links.ClearAllLinks();
                    Data.Children.Clear();
                });
            menu.ShowAsContext();
        }
    }
}
