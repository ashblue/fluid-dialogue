using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;

namespace CleverCrow.Fluid.Dialogues.Editors.Inspectors {
    [CustomEditor(typeof(NodeDataBase), true)]
    public class NodeDataBaseEditor : Editor {
        private ActionsSortableList _enterActions;

        private void OnEnable () {
            _enterActions = new ActionsSortableList(this, "enterActions");
        }

        public override void OnInspectorGUI () {
            base.OnInspectorGUI();

            _enterActions.Update();
        }
    }
}
