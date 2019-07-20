using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class ScrollManager {
        public const float WINDOW_SIZE = 30000;

        public Vector2 ScrollPos { get; set; }

        public void UpdateScrollView (Rect position) {
            ScrollPos = GUI.BeginScrollView(
                new Rect(0, 0, position.width, position.height),
                ScrollPos,
                new Rect(0, 0, WINDOW_SIZE, WINDOW_SIZE));
        }

        public void ResetViewToOrigin () {
            ScrollPos = new Vector2(WINDOW_SIZE / 2, WINDOW_SIZE / 2);
        }
    }
}
