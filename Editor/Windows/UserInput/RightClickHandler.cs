using System.Linq;
using CleverCrow.Fluid.Dialogues.Editors.NodeDisplays;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class RightClickHandler {
        private readonly DialogueWindow _window;
        private readonly NodeSelection _selection;
        private readonly ScrollManager _scroll;
        private readonly DelayedMenu _menu;

        private NodeEditorBase _clickedNode;
        private bool _isCameraDragging;
        private Connection _connection;
        private bool _bodyClick;

        public RightClickHandler (
            DialogueWindow window,
            NodeSelection selection,
            ScrollManager scroll,
            DelayedMenu menu) {
            _window = window;
            _selection = selection;
            _scroll = scroll;
            _menu = menu;
        }

        public void Update (Event e) {
            if (e.button != 1) return;

            if (e.type == EventType.MouseDown) {
                _connection = null;
                _clickedNode = null;
                _bodyClick = false;

                foreach (var node in _window.Nodes) {
                    var connection = node.GetConnection(e.mousePosition);
                    if (connection != null) {
                        _connection = node.GetConnection(e.mousePosition);
                        _clickedNode = node;
                        continue;
                    }

                    if (node.IsHeaderPosition(e.mousePosition)) {
                        _connection = null;
                        _clickedNode = node;
                    } else if (node.Data.rect.Contains(e.mousePosition)) {
                        _bodyClick = true;
                    }
                }
            }

            if (_connection != null) {
                GUI.FocusControl(null);
                ConnectionContextClick(e);
            } else if (_clickedNode == null && !_bodyClick) {
                GUI.FocusControl(null);
                EmptyContextClick(e);
            } else if (_clickedNode != null) {
                GUI.FocusControl(null);
                NodeContextClick(e);
            }

            if (e.type == EventType.MouseUp) {
                _clickedNode = null;
            }
        }

        private void ConnectionContextClick (Event e) {
            switch (e.type) {
                case EventType.MouseUp when _connection.IsClicked(e.mousePosition):
                    _connection.ShowContextMenu();
                    break;
            }
        }

        private void NodeContextClick (Event e) {
            switch (e.type) {
                case EventType.MouseDown when !_selection.Contains(_clickedNode):
                    _selection.RemoveAll();
                    _selection.Add(_clickedNode);
                    GUI.changed = true;
                    break;
                case EventType.MouseUp when _selection.Selected.Count == 1:
                    _clickedNode.ShowContextMenu();
                    break;
                case EventType.MouseUp:
                    ShowEditGroupMenu(e);
                    break;
            }
        }

        private void EmptyContextClick (Event e) {
            switch (e.type) {
                case EventType.MouseDrag:
                    _scroll.ScrollPos -= e.delta;
                    _isCameraDragging = true;
                    e.Use();
                    break;

                case EventType.MouseUp: {
                    var wasDragging = _isCameraDragging;
                    _isCameraDragging = false;

                    if (wasDragging) break;

                    _selection.RemoveAll();
                    GUI.changed = true;
                    _menu.Display = () => { ShowCanvasContextMenu(e); };

                    break;
                }
            }
        }

        private void ShowEditGroupMenu (Event e) {
            if (_selection.Selected.Any(i => i.Protected)) return;

            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Duplicate All"), false, () => {
                _window.GraphCrud.DuplicateNode(_selection.Selected);
            });
            menu.AddItem(new GUIContent("Delete All"), false, () => {
                _window.GraphCrud.DeleteNode(_selection.Selected);
            });

            menu.ShowAsContext();
        }

        private void ShowCanvasContextMenu (Event e) {
            var menu = new GenericMenu();
            var mousePosition = e.mousePosition;
            foreach (var menuLine in NodeAssemblies.StringToData) {
                menu.AddItem(new GUIContent(menuLine.Key), false, () => {
                    var data = ScriptableObject.CreateInstance(menuLine.Value);
                    _window.GraphCrud.CreateData(data as NodeDataBase, mousePosition);
                });
            }

            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Show Root"), false, () => {
                _scroll.SetViewToRect(_window.Graph.root.rect);
            });

            menu.ShowAsContext();
        }
    }
}
