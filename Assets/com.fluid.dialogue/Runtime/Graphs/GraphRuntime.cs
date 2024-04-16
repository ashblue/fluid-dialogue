using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Graphs {
    public class GraphRuntime : IGraph {
        private readonly Dictionary<INodeData, INode> _dataToRuntime;

        public INode Root { get; }
        public IGraphData Data { get; }

        public GraphRuntime (IDialogueController dialogue, IGraphData data) {
            _dataToRuntime = data.Nodes.ToDictionary(
                k => k,
                v => v.GetRuntime(this, dialogue));

            Root = GetCopy(data.Root);
            Data = data;
        }

        public INode GetCopy (INodeData original) {
            return _dataToRuntime[original];
        }

        public INode GetNodeByDataId (string id) {
            return _dataToRuntime.FirstOrDefault(n => n.Key.UniqueId == id).Value;
        }
    }
}
