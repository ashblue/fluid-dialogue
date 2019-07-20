using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class InputController {
        private readonly NodeSelection _selection;
        private readonly LeftClickHandler _leftClick;
        private readonly RightClickHandler _rightClick;
        private readonly DelayedMenu _delayedMenu = new DelayedMenu();

        public ScrollManager Scroll { get; } = new ScrollManager();

        public InputController (DialogueWindow window) {
            _selection = new NodeSelection(window);
            _leftClick = new LeftClickHandler(window, _selection);
            _rightClick = new RightClickHandler(window, _selection, Scroll, _delayedMenu);

            Scroll.ResetViewToOrigin();
        }

        public void ProcessCanvasEvent (Event e) {
            if (_delayedMenu.ShowDelayedMenu(e)) {
                return;
            }

            _leftClick.Update(e);
            _rightClick.Update(e);
        }

        public void PaintSelection () {
            _selection.PaintSelection();
        }
    }
}
