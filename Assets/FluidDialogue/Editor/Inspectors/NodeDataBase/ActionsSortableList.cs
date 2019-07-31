using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CleverCrow.Fluid.Dialogues.Actions;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.Inspectors {
    public class ActionsSortableList : SortableListBase {
        private static List<Type> _actionTypes;

        private static List<Type> ActionTypes =>
            _actionTypes ?? (_actionTypes = GetActionTypes());

        public ActionsSortableList (Editor editor, string property) : base(editor, property) {
        }

        protected override void OnInit () {
            _list.onAddDropdownCallback = (rect, list) => {
                var menu = new GenericMenu();

                foreach (var type in ActionTypes) {
                    var path = type.FullName;
                    var details = type.GetCustomAttribute<CreateActionMenuAttribute>();
                    if (details != null) path = details.Path;

                    menu.AddItem(
                        new GUIContent(path),
                        false,
                        () => Debug.Log("Create"));
                }

                menu.ShowAsContext();
            };
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
