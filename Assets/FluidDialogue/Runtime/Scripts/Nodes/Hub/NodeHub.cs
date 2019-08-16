using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Conditions;
using CleverCrow.Fluid.Dialogues.Graphs;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeHub : NodeBase {
        public NodeHub (
            IGraph graph,
            string uniqueId,
            List<INodeData> children,
            List<ICondition> conditions,
            List<IAction> enterActions,
            List<IAction> exitActions) :
            base(graph, uniqueId, children, conditions, enterActions, exitActions) {
        }
    }
}
