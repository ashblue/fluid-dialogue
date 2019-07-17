using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class MouseEventHandler {
        private readonly DialogueWindow _window;

        public MouseEventHandler (DialogueWindow window) {
            _window = window;
        }

        public void Poll () {
            var e = Event.current;

            if (e.type == EventType.MouseDrag && e.button == 1) {
                _window.ScrollPos -= e.delta;
                e.Use();
            }
        }
    }
}
