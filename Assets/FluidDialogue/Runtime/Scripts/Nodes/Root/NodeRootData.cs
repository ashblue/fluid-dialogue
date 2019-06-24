using System.Linq;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeRootData : NodeDataBase {
        public override INode GetRuntime () {
            return new NodeRoot(
                UniqueId,
                children.Select(c => c.GetRuntime()).ToList(),
                conditions.Select(c => c.GetRuntime()).ToList(),
                enterActions.Select(c => c.GetRuntime()).ToList(),
                exitActions.Select(c => c.GetRuntime()).ToList()
            );
        }
    }
}
