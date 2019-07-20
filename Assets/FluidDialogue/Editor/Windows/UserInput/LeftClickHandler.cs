using System.Linq;
using CleverCrow.Fluid.Dialogues.Editors.NodeDisplays;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class LeftClickHandler {
        private readonly DialogueWindow _window;
        private readonly NodeSelection _selection;

        private NodeDisplayBase _clickedNode;
        private bool _selectingArea;
        private bool _isDraggingNode;

        public LeftClickHandler (DialogueWindow window, NodeSelection selection) {
            _window = window;
            _selection = selection;
        }

        public void Update (Event e) {
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
            _selection.RemoveAll();
            var selected = _window.Nodes.Where(n => _selection.area.Overlaps(n.Data.rect));
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
                    Undo.SetCurrentGroupName("Delete nodes");
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

                    _isDraggingNode = false;
                    break;
            }
        }
    }
}
