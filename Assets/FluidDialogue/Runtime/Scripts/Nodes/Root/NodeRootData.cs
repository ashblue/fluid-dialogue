using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Actions;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeRootData : NodeDataBase {
        public List<NodeDataBase> children;
        public List<ActionDataBase> exitActions;

        public override INode GetRuntime () {
            return new NodeRoot(
                children.Select(c => c.GetRuntime()).ToList(),
                exitActions.Select(c => c.GetRuntime()).ToList()
            );
        }
    }
}
