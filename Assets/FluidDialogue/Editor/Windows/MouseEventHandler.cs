using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class MouseEventHandler {
        private readonly DialogueWindow _window;
        private bool _isDragging;

        public MouseEventHandler (DialogueWindow window) {
            _window = window;
        }

        public void Poll () {
            var e = Event.current;

            switch (e.type) {
                case EventType.MouseDrag when e.button == 1:
                    _window.ScrollPos -= e.delta;
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
    }
}
