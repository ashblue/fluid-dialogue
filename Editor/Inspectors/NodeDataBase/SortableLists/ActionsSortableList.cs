using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;

namespace CleverCrow.Fluid.Dialogues.Editors.Inspectors {
    public class ActionsSortableList : SortableListBase {
        private static TypesToMenu<ActionDataBase> _actionTypes;

        private readonly ScriptableObjectListPrinter _soPrinter;
        private readonly NestedDataCrud<ActionDataBase> _actionCrud;

        private static TypesToMenu<ActionDataBase> ActionTypes =>
            _actionTypes ??= new TypesToMenu<ActionDataBase>();

        public ActionsSortableList (Editor editor, string property, NodeDataBase node, List<ActionDataBase> actions)
            : base(editor, property) {
            _soPrinter = new ScriptableObjectListPrinter(editor.serializedObject.FindProperty(property));
            _actionCrud = new NestedDataCrud<ActionDataBase>(node, actions, ActionTypes);

            _list.drawElementCallback = _soPrinter.DrawScriptableObject;
            _list.elementHeightCallback = _soPrinter.GetHeight;

            _list.onAddDropdownCallback = _actionCrud.ShowMenu;
            _list.onRemoveCallback = _actionCrud.DeleteItem;
        }
    }
}
