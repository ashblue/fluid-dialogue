using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.Inspectors {
    public class SortableListBase {
        protected readonly Editor _editor;
        protected readonly ReorderableList _list;
        protected readonly SerializedObject _serializedObject;
        protected readonly SerializedProperty _serializedProp;

        public SortableListBase (Editor editor, string property) {
            if (editor == null) {
                Debug.LogError("Editor cannot be null");
                return;
            }

            _editor = editor;
            _serializedProp = _editor.serializedObject.FindProperty(property);
            _serializedObject = _editor.serializedObject;

            if (_serializedProp == null) {
                Debug.LogErrorFormat("Could not find property {0}", property);
                return;
            }

            _list = new ReorderableList(
                _serializedObject,
                _serializedProp,
                true, true, true, true);

            _list.drawHeaderCallback = rect => {
                EditorGUI.LabelField(rect, _serializedProp.displayName);
            };
        }

        public void Update () {
            _serializedObject.Update();

            _list?.DoLayoutList();

            _serializedObject.ApplyModifiedProperties();
        }
    }
}
