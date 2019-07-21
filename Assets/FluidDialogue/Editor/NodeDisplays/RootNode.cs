using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    [NodeType(typeof(NodeRootData))]
    public class RootNode : NodeDisplayBase {
        private Connection _out;

        public override bool Protected => true;

        protected override void OnSetup () {
            _out = CreateConnection(ConnectionType.Out);
        }

        protected override void OnUpdate () {
            var outPosition = Data.rect.position;
            outPosition.x += Data.rect.width - Connection.Size / 2;
            outPosition.y += Data.rect.height / 2 - Connection.Size / 2;
            _out.SetPosition(outPosition);
        }
    }
}
