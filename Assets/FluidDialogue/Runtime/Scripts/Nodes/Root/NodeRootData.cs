using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Conditions;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeRootData : NodeDataBase {
        public List<NodeDataBase> children;
        public List<ConditionDataBase> conditions;
        public List<ActionDataBase> enterActions;
        public List<ActionDataBase> exitActions;

        public override INode GetRuntime () {
            return new NodeRoot(
                children.Select(c => c.GetRuntime()).ToList(),
                conditions.Select(c => c.GetRuntime()).ToList(),
                enterActions.Select(c => c.GetRuntime()).ToList(),
                exitActions.Select(c => c.GetRuntime()).ToList()
            );
        }
    }
}
