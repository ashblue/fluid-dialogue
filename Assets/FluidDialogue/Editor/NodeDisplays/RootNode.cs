using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    [NodeType(typeof(NodeRootData))]
    public class RootNode : NodeDisplayBase {
        public override bool Protected => true;
    }
}
