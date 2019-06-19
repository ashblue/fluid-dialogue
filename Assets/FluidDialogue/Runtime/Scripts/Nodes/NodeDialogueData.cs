using System.Collections.Generic;
using System.Linq;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeDialogueData : NodeDataBase {
        public IActor actor;
        public string dialogue;
        public List<NodeDataBase> children;
        public List<IChoiceRuntime> choices;

        public override INodeRuntime GetRuntime () {
            var childrenRuntime = children.Select(c => c.GetRuntime()).ToList();
            return new NodeDialogue(actor, dialogue, childrenRuntime, choices);
        }
    }
}
