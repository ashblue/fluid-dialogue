using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.Inspectors {
    public class ActionsSortableList : SortableListBase {
        private static List<Type> _actionTypes;

        private readonly HashSet<string> _variableBlacklist = new HashSet<string> {
            "m_Script",
            "_uniqueId",
        };

        private static List<Type> ActionTypes =>
            _actionTypes ?? (_actionTypes = GetActionTypes());

        public ActionsSortableList (Editor editor, string property) : base(editor, property) {
        }

        protected override void OnInit () {
            _list.drawElementCallback = DrawScriptableObject;
            _list.elementHeightCallback = GetHeight;

            _list.onAddDropdownCallback = ShowActionMenu;
        }

        private void DrawScriptableObject (Rect rect, int index, bool active, bool focused) {
            var totalHeight = 0f;

            var element = _serializedProp.GetArrayElementAtIndex(index);
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

        private float GetHeight (int index) {
            var totalHeight = EditorGUIUtility.singleLineHeight;

            var element = _serializedProp.GetArrayElementAtIndex(index);
            var propIterator = new SerializedObject(element.objectReferenceValue).GetIterator();

            while (propIterator.NextVisible(true)) {
                if (_variableBlacklist.Contains(propIterator.name)) continue;

                var height = EditorGUI.GetPropertyHeight(propIterator);
                totalHeight += height;
            }

            return totalHeight;
        }

        private void ShowActionMenu (Rect buttonRect, ReorderableList list) {
            var menu = new GenericMenu();

            foreach (var type in ActionTypes) {
                var path = type.FullName;
                var details = type.GetCustomAttribute<CreateActionMenuAttribute>();
                if (details != null) path = details.Path;

                menu.AddItem(
                    new GUIContent(path),
                    false,
                    () => CreateAction(type));
            }

            menu.ShowAsContext();
        }

        private void CreateAction (Type actionType) {
            var node = _editor.target as NodeDataBase;
            Debug.Assert(node != null, "Failed to cast action parent object");

            var graphPath = AssetDatabase.GetAssetPath(node);
            var graph = AssetDatabase.LoadAssetAtPath<ScriptableObject>(graphPath);

            var action = ScriptableObject.CreateInstance(actionType) as ActionDataBase;
            Debug.Assert(action != null, $"Failed to create new action {actionType}");
            action.Setup();

            Undo.SetCurrentGroupName("Create node");

            Undo.RecordObject(graph, "Add Action");
            Undo.RecordObject(node, "Add Action");

            node.enterActions.Add(action);
            AssetDatabase.AddObjectToAsset(action, graph);
            AssetDatabase.SaveAssets();
            Undo.RegisterCreatedObjectUndo(action, "Add Action");

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        private static List<Type> GetActionTypes () {
            _actionTypes = Assembly
                .GetAssembly(typeof(ActionDataBase))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(ActionDataBase)) && !t.IsAbstract)
                .OrderByDescending(t => {
                    var attr = t.GetCustomAttribute<CreateActionMenuAttribute>();
                    return attr?.Priority ?? 0;
                })
                .ToList();

            return _actionTypes;
        }
    }
}
