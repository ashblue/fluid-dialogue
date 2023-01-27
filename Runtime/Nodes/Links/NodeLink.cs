using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Conditions;
using CleverCrow.Fluid.Dialogues.Graphs;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeLink : NodeBase {
        public override bool IsValid => Children[0]?.IsValid ?? false;

        public NodeLink (
            IGraph graph,
            string UniqueId,
            INodeData child,
            List<ICondition> conditions,
            List<IAction> enterActions,
            List<IAction> exitActions) :
            base(graph, UniqueId, new List<INodeData>{child}, conditions, enterActions, exitActions) {
        }
    }
}
