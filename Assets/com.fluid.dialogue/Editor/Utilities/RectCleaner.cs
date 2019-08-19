using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public static class RectCleaner {
        public static Rect FixNegativeSize (this Rect rectOld) {
            var rect = new Rect(rectOld);

            if (rect.width < 0) {
                rect.x += rect.width;
                rect.width = Mathf.Abs(rect.width);
            }

            if (rect.height < 0) {
                rect.y += rect.height;
                rect.height = Mathf.Abs(rect.height);
            }

            return rect;
        }
    }
}
