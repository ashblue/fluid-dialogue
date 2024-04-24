using UnityEditor;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public partial class DialogueWindow {
        private const string PREF_OPEN = "FluidDialogue_WindowOpen";

        private void Awake () {
            BindUndoRedoCallback();
            EditorPrefs.SetBool(PREF_OPEN, true);
            RestoreSavedGraph();
        }

        private void OnEnable () {
            BindUndoRedoCallback();
            EditorPrefs.SetBool(PREF_OPEN, true);
        }

        private void OnDestroy () {
            Undo.undoRedoPerformed -= UndoDetected;
            EditorPrefs.SetBool(PREF_OPEN, false);
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
