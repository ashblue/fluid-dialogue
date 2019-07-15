using CleverCrow.Fluid.Dialogues.Editors.NodeDisplays;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class MouseEventHandler {
        private readonly DialogueWindow _window;

        private Event _guiEvent;
        private bool _isClickEvent;
        private bool _repaint;

        public MouseEventHandler (DialogueWindow window) {
            _window = window;
        }

        public void BeginPoll () {
            _guiEvent = Event.current;
            _isClickEvent = _guiEvent.type == EventType.MouseDown;
            _repaint = false;
        }

        public void DetectClick (NodeDisplayBase node) {
            if (!_isClickEvent) return;

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
