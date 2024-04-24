using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes;
using NSubstitute;

namespace CleverCrow.Fluid.Dialogues.Builders {
    public class DialogueGraphStubBuilder {
        private readonly List<INodeData> _nodes = new List<INodeData>();
        private INode _next;

        public DialogueGraphStubBuilder WithNode (INodeData node) {
            _nodes.Add(node);
            return this;
        }

        public DialogueGraphStubBuilder WithNextResult (INode node) {
            _next = node;
            return this;
        }

        public IGraph Build () {
            var graph = Substitute.For<IGraph>();
            var root = A.Node
                .WithNextResult(_next)
                .WithPlayAction((playback) => playback.Next())
                .Build();
            graph.Root.Returns(root);

            var rootData = A.NodeData
                .WithNode(root)
                .Build();

            graph.GetCopy(rootData).Returns(root);

            foreach (var nodeData in _nodes) {
                graph
                    .GetCopy(nodeData)
                    .Returns(x => nodeData.GetRuntime(null, null));
            }

            return graph;
        }
    }
}
