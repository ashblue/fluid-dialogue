using CleverCrow.Fluid.Dialogues.Graphs;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    // @TODO This should inherit a simpler NodeDataBase that doesn't have children, conditions, enterActions, exitActions, ect.
    // This can be done by breaking down the base NodeDataBase into a separate NodeDataHierarchyBase class that inherits from a minimal NodeDataBase
    // The editor display will also need to be adjusted accordingly
    [CreateMenu("Note")]
    public class NodeNoteData : NodeDataBase {
        [TextArea]
        public string note;

        protected override string DefaultName => "Note";
        public override string Text => note;

        public override bool HideConnections => true;
        public override bool HideInspectorActions => true;
        public override bool HideInspectorConditions => true;

        public override INode GetRuntime (IGraph graphRuntime, IDialogueController controller) {
            // There is no runtime, this is an editor only note
            return null;
        }
    }
}
