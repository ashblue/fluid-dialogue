using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Conditions;
using CleverCrow.Fluid.Dialogues.Graphs;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public abstract class NodeBase : INode {
        private readonly List<INodeData> _children;
        private readonly List<ICondition> _conditions;
        private readonly IGraph _runtime;
        private List<INode> _childrenRuntimeCache;

        public List<IAction> EnterActions { get; }
        public List<IAction> ExitActions { get; }
        public virtual bool IsValid =>
            _conditions.Find(c => !c.GetIsValid(this)) == null;
        public List<IChoice> HubChoices { get; }
        public string UniqueId { get; }

        protected List<INode> Children =>
            _childrenRuntimeCache ??
            (_childrenRuntimeCache = _children.Select(_runtime.GetCopy).ToList());

        protected NodeBase (
            IGraph runtime,
            string uniqueId,
            List<INodeData> children,
            List<ICondition> conditions,
            List<IAction> enterActions,
            List<IAction> exitActions
        ) {
            _runtime = runtime;
            UniqueId = uniqueId;
            _children = children;
            _conditions = conditions;
            EnterActions = enterActions;
            ExitActions = exitActions;
        }

        public INode Next () {
            return Children.Find(n => n.IsValid);
        }

        public void Play (IDialoguePlayback playback) {
            playback.Events.NodeEnter.Invoke(this);
            OnPlay(playback);
        }

        protected virtual void OnPlay (IDialoguePlayback playback) {
            playback.Next();
        }

        public virtual IChoice GetChoice (int index) {
            throw new System.NotImplementedException();
        }
    }
}
