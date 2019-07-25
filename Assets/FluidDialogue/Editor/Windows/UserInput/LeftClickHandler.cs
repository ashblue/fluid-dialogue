using System.Linq;
using CleverCrow.Fluid.Dialogues.Editors.NodeDisplays;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class LeftClickHandler {
        private readonly DialogueWindow _window;
        private readonly NodeSelection _selection;

        private NodeDisplayBase _clickedNode;
        private bool _selectingArea;
        private bool _isDraggingNode;
        private Connection _connection;

        public LeftClickHandler (DialogueWindow window, NodeSelection selection) {
            _window = window;
            _selection = selection;
        }

        public void Update (Event e) {
            if (e.button != 0) return;

            if (e.type == EventType.MouseDown) {
                foreach (var node in _window.Nodes) {
                    _connection = node.GetConnection(e.mousePosition);
                    if (_connection != null) {
                        _clickedNode = node;
                        break;
                    }

                    if (node.Data.rect.Contains(e.mousePosition)) {
                        _clickedNode = node;
                        break;
                    }
                }
            }

            if (_connection != null) {
                UpdateClickedConnection(e);
            } else if (_clickedNode == null) {
                DrawSelectionArea(e);
            } else if (_clickedNode != null) {
                UpdateClickedNode(e);
            }

            if (e.type == EventType.MouseUp) {
                _clickedNode = null;
            }
        }

        private void UpdateClickedConnection (Event e) {
            switch (e.type) {
                case EventType.MouseDrag:
                    _connection.SetExampleCurve(e.mousePosition);
                    break;
                case EventType.MouseUp:
                    Connection linkTarget = null;
                    foreach (var node in _window.Nodes) {
                        linkTarget = node.GetConnection(e.mousePosition);
                        if (linkTarget != null) break;
                    }

                    _connection.Links.AddLink(linkTarget);
                    _connection.ClearCurveExample();

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
                    SetSelection();
                    break;

                case EventType.Ignore:
                    SetSelection();
                    break;
            }
        }

        private void SetSelection () {
            var cleanedSelectArea = _selection.area.FixNegativeSize();

            _selection.RemoveAll();
            var selected = _window.Nodes.Where(n => cleanedSelectArea.Overlaps(n.Data.rect));
            _selection.Add(selected);

            _selection.area.size = Vector2.zero;
            _selectingArea = false;
            GUI.changed = true;
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
                    Undo.SetCurrentGroupName("Drag nodes");
                    _selection.Selected.ForEach(n => {
                        Undo.RegisterCompleteObjectUndo(n.Data, "Move node");
                        n.Data.rect.position += e.delta;
                    });
                    Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
                    e.Use();
                    break;

                case EventType.MouseUp:
                    if (!_isDraggingNode) {
                        _selection.RemoveAll();
                        _selection.Add(_clickedNode);
                        GUI.changed = true;
                    }

                    ClearDragging();
                    break;

                case EventType.Ignore:
                    ClearDragging();
                    break;
            }
        }

        private void ClearDragging () {
            if (!_isDraggingNode) return;

            Undo.SetCurrentGroupName("Drag nodes");
            foreach (var node in _selection.Selected) {
                foreach (var link in node.In.Parents) {
                    Undo.RegisterCompleteObjectUndo(
                        link.Data as NodeDataBase, "Move node");
                    link.Data.SortChildrenByPosition();
                }
            }
            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

            _isDraggingNode = false;
        }
    }
}
