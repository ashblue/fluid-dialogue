using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    [NodeType(typeof(NodeRootData))]
    public class RootEditor : NodeEditorBase {
        private Connection _out;

        public override bool Protected => true;
        protected override bool HasInConnection => false;

        protected override void OnSetup () {
        }
    }
}
