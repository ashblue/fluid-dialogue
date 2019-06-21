using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Conditions;

namespace CleverCrow.Fluid.Dialogues.Nodes.Hub {
    public class NodeHub : INode {
        private readonly List<INode> _children;
        private readonly List<ICondition> _conditions;

        public List<IAction> EnterActions { get; }
        public List<IAction> ExitActions { get; }
        public bool IsValid => _conditions.Find(c => !c.GetIsValid()) == null;
        public List<IChoice> HubChoices { get; }

        public NodeHub (
            List<INode> children,
            List<ICondition> conditions,
            List<IAction> actions
        ) {
            _children = children;
            _conditions = conditions;
            EnterActions = actions;
        }

        public INode Next () {
            return _children.Find(n => n.IsValid);
        }

        public void Play (IDialoguePlayback playback) {
            playback.Next();
        }

        public IChoice GetChoice (int index) {
            throw new System.NotImplementedException();
        }
    }
}
