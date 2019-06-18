using System.Collections.Generic;
using System.Linq;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeDialogueData : NodeDataBase {
        public List<NodeDataBase> children;

        public override INodeRuntime GetRuntime () {
            var childrenRuntime = children.Select(c => c.GetRuntime()).ToList();
            return new NodeDialogue(childrenRuntime);
        }
    }
}
