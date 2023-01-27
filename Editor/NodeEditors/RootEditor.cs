using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    [NodeType(typeof(NodeRootData))]
    public class RootEditor : NodeEditorBase {
        public override bool Protected => true;
        protected override bool HasInConnection => false;
    }
}
