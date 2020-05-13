using CleverCrow.Fluid.FindAndReplace.Editors;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class DialogueSearchResult : IFindResult {
        private readonly Object _target;
        public string Text { get; }

        public DialogueSearchResult (string text, Object target) {
            _target = target;
            Text = text;
        }

        public void Show () {
            Selection.activeObject = _target;;
        }

        public void Replace (int startIndex, int charactersToReplace, string replaceText) {
            var beginning = Text.Substring(0, startIndex);
            var end = Text.Substring(startIndex + charactersToReplace);
            var text = $"{beginning}{replaceText}{end}";

            // @NOTE Mixed feels on this, seems risky targeting the dialogue prop directly
            var obj = new SerializedObject(_target);
            obj.FindProperty("dialogue").stringValue = text;
            obj.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();
        }
    }
}
