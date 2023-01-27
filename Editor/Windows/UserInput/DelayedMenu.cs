using System;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class DelayedMenu {
        public Action Display { private get; set; }

        public bool ShowDelayedMenu (Event e) {
            if (Display == null || e.type != EventType.Repaint) return false;

            Display.Invoke();
            Display = null;
            return true;
        }
    }
}
