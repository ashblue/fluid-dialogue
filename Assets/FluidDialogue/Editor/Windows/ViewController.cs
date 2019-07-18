using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class ViewController {
        public const float WINDOW_SIZE = 30000;

        private readonly DialogueWindow _window;
        private bool _isDragging;

        private Vector2 ScrollPos { get; set; }

        public ViewController (DialogueWindow window) {
            _window = window;
            ResetViewToOrigin();
        }

        public void ProcessCanvasEvent (Event e) {
            switch (e.type) {
                case EventType.MouseDrag when e.button == 1:
                    ScrollPos -= e.delta;
                    _isDragging = true;
                    e.Use();
                    break;
                case EventType.MouseUp when e.button == 1: {
                    var wasDragging = _isDragging;
                    _isDragging = false;

                    if (wasDragging) return;

                    ShowContextMenu(e);

                    _isDragging = false;
                    break;
                }
            }
        }

        public void UpdateScrollView (Rect position) {
            ScrollPos = GUI.BeginScrollView(
                new Rect(0, 0, position.width, position.height),
                ScrollPos,
                new Rect(0, 0, WINDOW_SIZE, WINDOW_SIZE));
        }

        private void ShowContextMenu (Event e) {
            var menu = new GenericMenu();
            var mousePosition = e.mousePosition;
            foreach (var menuLine in NodeAssemblies.StringToData) {
                menu.AddItem(new GUIContent(menuLine.Key), false, () => {
                    var data = ScriptableObject.CreateInstance(menuLine.Value);
                    _window.CreateData(data as NodeDataBase, mousePosition);
                });
            }

            menu.ShowAsContext();
        }


        private void ResetViewToOrigin () {
            ScrollPos = new Vector2(WINDOW_SIZE / 2, WINDOW_SIZE / 2);
        }
    }
}
