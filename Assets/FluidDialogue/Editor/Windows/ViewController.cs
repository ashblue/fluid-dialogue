using System;
using CleverCrow.Fluid.Dialogues.Editors.NodeDisplays;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    // @TODO Selection handling as its own class
    public class ViewController {
        public const float WINDOW_SIZE = 30000;
        private readonly DialogueWindow _window;
        private readonly NodeSelection _selection;

        private Action _delayedContextMenu;
        private bool _selectingArea;
        private NodeDisplayBase _clickedNode;
        private bool _isDraggingNode;
        private bool _isCameraDragging;

        private Vector2 ScrollPos { get; set; }

        public ViewController (DialogueWindow window) {
            _window = window;
            _selection = new NodeSelection(window);
            ResetViewToOrigin();
        }

        public void ProcessCanvasEvent (Event e) {
            if (_delayedContextMenu != null && e.type == EventType.Repaint) {
                _delayedContextMenu.Invoke();
                _delayedContextMenu = null;
                return;
            }

            LeftClickHandler(e);
            RightClickHandler(e);

            _selection.PaintSelection();
        }

        private void RightClickHandler (Event e) {
            if (e.button != 1) return;

            if (e.type == EventType.MouseDown) {
                _clickedNode = _window.Nodes.Find(n => n.Data.rect.Contains(e.mousePosition));
            }

            if (_clickedNode == null) {
                EmptyContextClick(e);
            } else if (_clickedNode != null) {
                NodeContextClick(e);
            }

            if (e.type == EventType.MouseUp) {
                _clickedNode = null;
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
                    Debug.Log("Show single selection context menu");
                    break;
                case EventType.MouseUp:
                    Debug.Log("Show group selection context menu");
                    break;
            }
        }

        private void EmptyContextClick (Event e) {
            switch (e.type) {
                case EventType.MouseDrag:
                    ScrollPos -= e.delta;
                    _isCameraDragging = true;
                    e.Use();
                    break;

                case EventType.MouseUp: {
                    var wasDragging = _isCameraDragging;
                    _isCameraDragging = false;

                    if (wasDragging) break;

                    _selection.RemoveAll();
                    GUI.changed = true;
                    _delayedContextMenu = () => { ShowContextMenu(e); };

                    break;
                }
            }
        }

        private void LeftClickHandler (Event e) {
            if (e.button != 0) return;

            if (e.type == EventType.MouseDown) {
                _clickedNode = _window.Nodes.Find(n => n.Data.rect.Contains(e.mousePosition));
            }

            if (_clickedNode == null) {
                DrawSelectionArea(e);
            } else if (_clickedNode != null) {
                UpdateClickedNode(e);
            }

            if (e.type == EventType.MouseUp) {
                _clickedNode = null;
            }
        }

        private void UpdateClickedNode (Event e) {
            switch (e.type) {
                case EventType.MouseDown when !_selection.Contains(_clickedNode):
                    _selection.RemoveAll();
                    _selection.Add(_clickedNode);
                    GUI.changed = true;
                    break;

                case EventType.MouseDrag:
                    _isDraggingNode = true;
                    _selection.Selected.ForEach(n => n.Data.rect.position += e.delta);
                    e.Use();
                    break;

                case EventType.MouseUp:
                    if (!_isDraggingNode) {
                        _selection.RemoveAll();
                        _selection.Add(_clickedNode);
                        GUI.changed = true;
                    }

                    _isDraggingNode = false;
                    break;
            }
        }

        private void DrawSelectionArea (Event e) {
            switch (e.type) {
                case EventType.MouseDown:
                    _selectingArea = true;
                    _selection.area.position = e.mousePosition;
                    _selection.area.size = Vector2.zero;
                    break;

                case EventType.MouseDrag when _selectingArea:
                    _selection.area.size += e.delta;
                    break;

                case EventType.MouseUp when _selectingArea:
                    _selection.RemoveAll();
                    _window.Nodes.ForEach(n => {
                        if (_selection.area.Overlaps(n.Data.rect)) {
                            _selection.Add(n);
                        }
                    });

                    GUI.changed = true;
                    _selection.area.size = Vector2.zero;
                    _selectingArea = false;
                    break;
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
