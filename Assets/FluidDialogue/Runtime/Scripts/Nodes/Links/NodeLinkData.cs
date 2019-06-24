using System.Linq;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeLinkData : NodeDataBase {
        public override INode GetRuntime () {
            return new NodeLink(
                UniqueId,
                children.Count > 0 ? children[0].GetRuntime() : null,
                conditions.Select(c => c.GetRuntime()).ToList(),
                enterActions.Select(c => c.GetRuntime()).ToList(),
                exitActions.Select(c => c.GetRuntime()).ToList()
            );
        }
    }
}
