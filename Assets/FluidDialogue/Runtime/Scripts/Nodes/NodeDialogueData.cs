using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Conditions;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeDialogueData : NodeDataBase {
        public IActor actor;
        public string dialogue;
        public List<NodeDataBase> children;
        public List<IChoiceRuntime> choices;
        public List<ICondition> conditions;
        public List<IAction> enterActions;
        public List<IAction> exitActions;

        public override INodeRuntime GetRuntime () {
            var childrenRuntime = children.Select(c => c.GetRuntime()).ToList();
            return new NodeDialogue(
                actor,
                dialogue,
                childrenRuntime,
                choices,
                conditions,
                enterActions,
                exitActions);
        }
    }
}
