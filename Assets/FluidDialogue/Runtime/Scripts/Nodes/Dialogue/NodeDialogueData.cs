using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Conditions;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeDialogueData : NodeDataBase {
        public IActor actor;
        public string dialogue;
        public List<NodeDataBase> children;
        public List<ChoiceData> choices;
        public List<ConditionDataBase> conditions;
        public List<ActionDataBase> enterActions;
        public List<ActionDataBase> exitActions;

        public override INode GetRuntime () {
            return new NodeDialogue(
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
