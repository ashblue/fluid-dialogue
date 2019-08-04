using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;

namespace CleverCrow.Fluid.Dialogues.Editors.Inspectors {
    [CustomEditor(typeof(NodeDataBase), true)]
    public class NodeDataBaseEditor : Editor {
        private ActionsSortableList _enterActions;
        private ActionsSortableList _exitActions;

        private void OnEnable () {
            var node = target as NodeDataBase;
            _enterActions = new ActionsSortableList(this, "enterActions", node, node.enterActions);
            _exitActions = new ActionsSortableList(this, "exitActions", node, node.exitActions);

        }

        public override void OnInspectorGUI () {
            base.OnInspectorGUI();

            _enterActions.Update();
            _exitActions.Update();
        }
    }
}
