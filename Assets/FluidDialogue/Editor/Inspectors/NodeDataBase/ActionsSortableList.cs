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
        private ScriptableObjectListPrinter _soPrinter;

        private static List<Type> ActionTypes =>
            _actionTypes ?? (_actionTypes = GetActionTypes());

        public ActionsSortableList (Editor editor, string property) : base(editor, property) {
        }

        protected override void OnInit () {
            _soPrinter = new ScriptableObjectListPrinter(_serializedProp);

            _list.drawElementCallback = _soPrinter.DrawScriptableObject;
            _list.elementHeightCallback = _soPrinter.GetHeight;

            _list.onAddDropdownCallback = ShowActionMenu;
            _list.onRemoveCallback = DeleteAction;
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
            var graphPath = AssetDatabase.GetAssetPath(node);
            var graph = AssetDatabase.LoadAssetAtPath<ScriptableObject>(graphPath);

            var action = ScriptableObject.CreateInstance(actionType) as ActionDataBase;
            Debug.Assert(action != null, $"Failed to create new action {actionType}");
            action.Setup();

            Undo.SetCurrentGroupName("Add action");

            Undo.RecordObject(graph, "Add action");
            Undo.RecordObject(node, "Add action");

            node.enterActions.Add(action);
            AssetDatabase.AddObjectToAsset(action, graph);
            Undo.RegisterCreatedObjectUndo(action, "Add action");

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
            AssetDatabase.SaveAssets();
        }

        private void DeleteAction (ReorderableList list) {
            var node = _editor.target as NodeDataBase;
            var graphPath = AssetDatabase.GetAssetPath(node);
            var graph = AssetDatabase.LoadAssetAtPath<ScriptableObject>(graphPath);
            var action = node.enterActions[list.index];

            Undo.SetCurrentGroupName("Delete Action");

            Undo.RecordObject(graph, "Delete action");
            Undo.RecordObject(node, "Delete action");

            node.enterActions.Remove(action);
            Undo.DestroyObjectImmediate(action);

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
            AssetDatabase.SaveAssets();
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
