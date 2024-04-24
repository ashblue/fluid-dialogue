using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.Inspectors {
    public class SortableListBase {
        protected ReorderableList _list;
        bool _skipFrame;

        public SortableListBase (Editor editor, string property) {
            if (editor == null) {
                Debug.LogError("Editor cannot be null");
                return;
            }

            var prop = editor.serializedObject.FindProperty(property);
            if (prop == null) {
                Debug.LogErrorFormat("Could not find property {0}", property);
                return;
            }

            _list = new ReorderableList(
                editor.serializedObject,
                prop,
                true, true, true, true);

            var title = prop.displayName;
            _list.drawHeaderCallback = rect => {
                EditorGUI.LabelField(rect, title);
            };
        }

        public void Update () {
            if (_list != null) _list.DoLayoutList();
        }
    }
}
