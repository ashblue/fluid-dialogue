using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Conditions;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeHub : NodeBase {
        public NodeHub (
            string uniqueId,
            List<INode> children,
            List<ICondition> conditions,
            List<IAction> enterActions,
            List<IAction> exitActions) :
            base(uniqueId, children, conditions, enterActions, exitActions) {
        }
    }
}
