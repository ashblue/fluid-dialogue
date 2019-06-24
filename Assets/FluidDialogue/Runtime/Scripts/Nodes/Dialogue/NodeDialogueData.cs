using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Choices;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeDialogueData : NodeDataBase {
        public IActor actor;
        public string dialogue;
        public List<ChoiceData> choices;

        public override INode GetRuntime () {
            return new NodeDialogue(
                UniqueId,
                actor,
                dialogue,
                children.Select(c => c.GetRuntime()).ToList(),
                choices.Select(c => c.GetRuntime()).ToList(),
                conditions.Select(c => c.GetRuntime()).ToList(),
                enterActions.Select(a => a.GetRuntime()).ToList(),
                exitActions.Select(a => a.GetRuntime()).ToList()
            );
        }
    }
}
