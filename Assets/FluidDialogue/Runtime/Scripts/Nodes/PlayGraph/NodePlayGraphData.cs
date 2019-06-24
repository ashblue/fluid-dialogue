using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Conditions;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes.PlayGraph;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodePlayGraphData : NodeDataBase {
        public GraphData graph;

        public override INode GetRuntime () {
            return new NodePlayGraph(
                UniqueId,
                graph,
                children.Select(c => c.GetRuntime()).ToList(),
                conditions.Select(c => c.GetRuntime()).ToList(),
                enterActions.Select(c => c.GetRuntime()).ToList(),
                exitActions.Select(c => c.GetRuntime()).ToList()
            );
        }
    }
}
