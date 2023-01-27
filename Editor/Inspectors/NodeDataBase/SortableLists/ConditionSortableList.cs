using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Conditions;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;

namespace CleverCrow.Fluid.Dialogues.Editors.Inspectors {
    public class ConditionSortableList : SortableListBase {
        private static TypesToMenu<ConditionDataBase> _conditionTypes;

        private ScriptableObjectListPrinter _soPrinter;
        private readonly NestedDataCrud<ConditionDataBase> _conditionCrud;

        private static TypesToMenu<ConditionDataBase> ConditionTypes =>
            _conditionTypes ?? (_conditionTypes = new TypesToMenu<ConditionDataBase>());

        public ConditionSortableList (Editor editor, string property, NodeDataBase node, List<ConditionDataBase> conditions)
            : base(editor, property) {
            _soPrinter = new ScriptableObjectListPrinter(_serializedProp);
            _conditionCrud = new NestedDataCrud<ConditionDataBase>(node, conditions, ConditionTypes);

            _list.drawElementCallback = _soPrinter.DrawScriptableObject;
            _list.elementHeightCallback = _soPrinter.GetHeight;

            _list.onAddDropdownCallback = _conditionCrud.ShowMenu;
            _list.onRemoveCallback = _conditionCrud.DeleteItem;
        }
    }
}
