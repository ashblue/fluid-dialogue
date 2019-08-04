using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;

namespace CleverCrow.Fluid.Dialogues.Editors.Inspectors {
    public class ActionsSortableList : SortableListBase {
        private readonly ScriptableObjectListPrinter _soPrinter;
        private readonly ActionCrud _actionCrud;

        public ActionsSortableList (Editor editor, string property, NodeDataBase node, List<ActionDataBase> actions)
            : base(editor, property) {
            _soPrinter = new ScriptableObjectListPrinter(_serializedProp);
            _actionCrud = new ActionCrud(node, actions);

            _list.drawElementCallback = _soPrinter.DrawScriptableObject;
            _list.elementHeightCallback = _soPrinter.GetHeight;

            _list.onAddDropdownCallback = _actionCrud.ShowActionMenu;
            _list.onRemoveCallback = _actionCrud.DeleteAction;
        }
    }
}
