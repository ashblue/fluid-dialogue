using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.Inspectors {
    public class ScriptableObjectListPrinter {
        private readonly SerializedProperty _serializedProp;

        private readonly HashSet<string> _variableBlacklist = new HashSet<string> {
            "m_Script",
            "_uniqueId",
        };

        public ScriptableObjectListPrinter (SerializedProperty serializedProp) {
            _serializedProp = serializedProp;
        }

        public void DrawScriptableObject (Rect rect, int index, bool active, bool focused) {
            var totalHeight = 0f;

            var element = _serializedProp.GetArrayElementAtIndex(index);
            if (element.objectReferenceValue == null) {
                Debug.LogWarning($"Null element detected in sortable list {element.name}");
                return;
            }

            var serializedObject = new SerializedObject(element.objectReferenceValue);
            var propIterator = serializedObject.GetIterator();

            EditorGUI.BeginChangeCheck();
            while (propIterator.NextVisible(true)) {
                if (_variableBlacklist.Contains(propIterator.name)) continue;

                var position = new Rect(rect);
                position.y += totalHeight;
                EditorGUI.PropertyField(position, propIterator, true);

                var height = EditorGUI.GetPropertyHeight(propIterator);
                totalHeight += height;
            }
            if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
        }

        public float GetHeight (int index) {
            var totalHeight = EditorGUIUtility.singleLineHeight;

            var element = _serializedProp.GetArrayElementAtIndex(index);
            if (element.objectReferenceValue == null) {
                Debug.LogWarning($"Null element detected in sortable list {element.name}");
                return 0;
            }

            var propIterator = new SerializedObject(element.objectReferenceValue).GetIterator();

            while (propIterator.NextVisible(true)) {
                if (_variableBlacklist.Contains(propIterator.name)) continue;

                var height = EditorGUI.GetPropertyHeight(propIterator);
                totalHeight += height;
            }

            return totalHeight;
        }
    }
}
