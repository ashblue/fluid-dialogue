using CleverCrow.Fluid.Dialogues.Nodes;
using NSubstitute;

namespace CleverCrow.Fluid.Dialogues.Builders {
    public class NodeDataStubBuilder {
        private INode _node;

        public NodeDataStubBuilder WithNode (INode node) {
            _node = node;
            return this;
        }

        public INodeData Build () {
            var nodeData = Substitute.For<INodeData>();
            nodeData.GetRuntime(null, null).ReturnsForAnyArgs(_node);

            return nodeData;
        }
    }
}
