using System;
using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CleverCrow.Fluid.Dialogues.Editors.Inspectors {
    public class NestedDataCrud<T> where T : Object, ISetup {
        private readonly NodeDataBase _node;
        private readonly List<T> _list;
        private readonly TypesToMenu<T> _menuData;

        public NestedDataCrud (NodeDataBase node, List<T> list, TypesToMenu<T> menuData) {
            _menuData = menuData;
            _node = node;
            _list = list;
        }

        public void ShowMenu (Rect buttonRect, ReorderableList list) {
            var menu = new GenericMenu();

            foreach (var line in _menuData.Lines) {
                menu.AddItem(
                    new GUIContent(line.path),
                    false,
                    () => CreateItem(line.type));
            }

            menu.ShowAsContext();
        }

        private void CreateItem (Type type) {
            var graphPath = AssetDatabase.GetAssetPath(_node);
            var graph = AssetDatabase.LoadAssetAtPath<ScriptableObject>(graphPath);

            var listItem = ScriptableObject.CreateInstance(type) as T;
            Debug.Assert(listItem != null, $"Failed to create new type {type}");
            listItem.Setup();

            if (FluidDialogueSettings.Current.HideNestedNodeData) {
                listItem.hideFlags = HideFlags.HideInHierarchy;
            }

            Undo.SetCurrentGroupName("Add type");

            Undo.RecordObject(graph, "Add type");
            Undo.RecordObject(_node, "Add type");

            _list.Add(listItem);
            AssetDatabase.AddObjectToAsset(listItem, graph);
            Undo.RegisterCreatedObjectUndo(listItem, "Add type");

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
            AssetDatabase.SaveAssets();
        }

        public void DeleteItem (ReorderableList list) {
            var graphPath = AssetDatabase.GetAssetPath(_node);
            var graph = AssetDatabase.LoadAssetAtPath<ScriptableObject>(graphPath);
            var listItem = _list[list.index];

            Undo.SetCurrentGroupName("Delete type");

            Undo.RecordObject(graph, "Delete type");
            Undo.RecordObject(_node, "Delete type");

            _list.Remove(listItem);
            Undo.DestroyObjectImmediate(listItem);

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
            AssetDatabase.SaveAssets();
        }
    }
}
