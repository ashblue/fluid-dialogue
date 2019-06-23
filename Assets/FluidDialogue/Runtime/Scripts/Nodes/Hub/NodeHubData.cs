using System.Linq;

namespace CleverCrow.Fluid.Dialogues.Nodes.Hub {
    public class NodeHubData : NodeDataBase {
        public override INode GetRuntime () {
            return new NodeHub(
                children.Select(c => c.GetRuntime()).ToList(),
                conditions.Select(c => c.GetRuntime()).ToList(),
                enterActions.Select(c => c.GetRuntime()).ToList(),
                exitActions.Select(c => c.GetRuntime()).ToList()
            );
        }
    }
}
