using UnityEditor;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public partial class DialogueWindow {
        private void Awake () {
            BindUndoRedoCallback();
        }

        private void OnEnable () {
            BindUndoRedoCallback();
        }

        private void OnDestroy () {
            Undo.undoRedoPerformed -= UndoDetected;
        }

        private void BindUndoRedoCallback () {
            Undo.undoRedoPerformed -= UndoDetected;
            Undo.undoRedoPerformed += UndoDetected;
        }

        private void UndoDetected () {
            Repaint();
        }
    }
}
