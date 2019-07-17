using CleverCrow.Fluid.Dialogues.Editors.NodeDisplays;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class MouseEventHandler {
        private readonly DialogueWindow _window;

        private Event _guiEvent;
        private bool _isPrimaryClick;
        private bool _repaint;
        private bool _isDragging;

        public MouseEventHandler (DialogueWindow window) {
            _window = window;
        }

        public void BeginPoll () {
            _guiEvent = Event.current;
            _repaint = false;

            _isPrimaryClick = _guiEvent.type == EventType.MouseDown;

            if (_guiEvent.type == EventType.MouseDrag && _guiEvent.button == 1) {
                _window.ScrollPos -= _guiEvent.delta;
                _guiEvent.Use();
            }
        }

        public void DetectClick (NodeDisplayBase node) {
            if (!_isPrimaryClick) return;

            if (node.Data.rect.Contains(_guiEvent.mousePosition)) {
                node.Select();
                _repaint = true;
            } else if (node.IsSelected) {
                node.Deselect();
                _repaint = true;
            }
        }

        public void EndPoll () {
            if (_repaint) _window.Repaint();
        }
    }
}
