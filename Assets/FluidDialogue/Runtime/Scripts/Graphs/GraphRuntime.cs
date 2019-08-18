using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Graphs {
    public class GraphRuntime : IGraph {
        private readonly Dictionary<INodeData, INode> _dataToRuntime;

        public INode Root { get; }

        public GraphRuntime (IDialogueController dialogue, IGraphData data) {
            _dataToRuntime = data.Nodes.ToDictionary(
                k => k,
                v => v.GetRuntime(this, dialogue));

            Root = GetCopy(data.Root);
        }

        public INode GetCopy (INodeData original) {
            return _dataToRuntime[original];
        }
    }
}
