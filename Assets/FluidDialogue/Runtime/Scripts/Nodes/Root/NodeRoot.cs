using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Choices;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeRoot : INode {
        private List<INode> _children;
        public List<IAction> ExitActions { get; }
        public List<IAction> EnterActions { get; }
        public bool IsValid => true;

        public NodeRoot (List<INode> children, List<IAction> exitActions) {
            _children = children;
            ExitActions = exitActions;
        }

        public INode Next () {
            return _children.Find(c => c.IsValid);
        }

        public void Play (IDialoguePlayback playback) {
            throw new System.NotImplementedException();
        }

        public IChoice GetChoice (int index) {
            throw new System.NotImplementedException();
        }
    }
}
