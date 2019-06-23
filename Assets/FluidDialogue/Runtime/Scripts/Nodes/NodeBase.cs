using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Conditions;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public abstract class NodeBase : INode {
        protected readonly List<INode> _children;
        private readonly List<ICondition> _conditions;

        public List<IAction> EnterActions { get; }
        public List<IAction> ExitActions { get; }
        public virtual bool IsValid =>
            _conditions.Find(c => !c.GetIsValid()) == null;
        public List<IChoice> HubChoices { get; }

        protected NodeBase (
            List<INode> children,
            List<ICondition> conditions,
            List<IAction> enterActions,
            List<IAction> exitActions
        ) {
            _children = children;
            _conditions = conditions;
            EnterActions = enterActions;
            ExitActions = exitActions;
        }

        public INode Next () {
            return _children.Find(n => n.IsValid);
        }

        public virtual void Play (IDialoguePlayback playback) {
            playback.Next();
        }

        public virtual IChoice GetChoice (int index) {
            throw new System.NotImplementedException();
        }
    }
}
