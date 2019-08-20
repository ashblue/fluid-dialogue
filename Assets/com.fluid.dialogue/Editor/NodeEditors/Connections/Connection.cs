using System;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    public partial interface IConnection {
        ConnectionType Type { get; }
        Rect Rect { get; }
        NodeDataBase Data { get; }
        IConnectionLinks Links { get; }
        bool IsFirst { get; }
        void UndoRecordAllObjects ();
        bool IsValidLinkTarget (IConnection target);
    }

    public partial class Connection : IConnection {
        public const float SIZE = 16;
        private static Texture2D _graphic;

        private Rect _rect = new Rect(Vector2.zero, new Vector2(SIZE, SIZE));
        private readonly IConnectionChildCollection _childCollection;

        public Func<IConnection, bool> IsValidLinkTargetCallback { private get; set; } = (i) => true;
        public ConnectionType Type { get; }
        public Rect Rect => _rect;
        public NodeDataBase Data { get; }
        public IConnectionLinks Links { get; }
        public bool IsFirst { get; }

        public IDialogueWindow Window { get; }

        private static Texture2D Graphic {
            get {
                if (_graphic == null) _graphic = Resources.Load<Texture2D>("dot");
                return _graphic;
            }
        }

        private bool IsMemoryLeak => _childCollection.Children.Count != Links.List.Count;
        public bool Hide { private get; set; }

        public Connection (ConnectionType type, NodeDataBase data, IConnectionChildCollection childCollection, IDialogueWindow window, bool isFirst) {
            IsFirst = isFirst;
            Window = window;
            Type = type;
            Data = data;
            _childCollection = childCollection;
            Links = new ConnectionLinks(this, childCollection);
        }

        public void Print () {
            if (Hide) return;

            if (IsMemoryLeak) {
                Links.RebuildLinks();
            }

            var color = GUI.color;
            GUI.color = GetGraphicColor();

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
            return !Hide && _rect.Contains(mousePosition);
        }

        public void SetPosition (Vector2 position) {
            _rect.position = position;
        }

        public void ShowContextMenu () {
            if (Type == ConnectionType.In) return;

            var menu = new GenericMenu();
            menu.AddItem(
                new GUIContent("Clear Connections"), false, () => {
                    Undo.RecordObject(_childCollection as Object, "Clear connections");
                    Links.ClearAllLinks();
                    _childCollection.ClearConnectionChildren();
                    DialogueWindow.SaveGraph();
                });
            menu.ShowAsContext();
        }

        public void UndoRecordAllObjects () {
            // Sometimes goes null when bulk deleting nodes
            if (Data == null) return;

            Undo.RecordObject(Data, "Changed connection");
            if (!(_childCollection is NodeDataBase)) {
                Undo.RecordObject(_childCollection as Object, "Changed connection");
            }
        }

        private Color GetGraphicColor () {
            switch (Type) {
                case ConnectionType.In:
                    var color = Color.gray;
                    color.a = 1f;
                    return color;
                case ConnectionType.Out:
                    return Color.cyan;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }
        public bool IsValidLinkTarget (IConnection target) {
            return IsValidLinkTargetCallback.Invoke(target);
        }
    }
}
