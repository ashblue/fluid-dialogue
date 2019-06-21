using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Choices;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeLink : INode {
        private readonly INode _child;

        public List<IAction> ExitActions { get; }
        public List<IAction> EnterActions { get; }
        public bool IsValid => _child.IsValid;
        public List<IChoice> HubChoices { get; }

        public NodeLink (INode child) {
            _child = child;
        }

        public INode Next () {
            return IsValid ? _child : null;
        }

        public void Play (IDialoguePlayback playback) {
            playback.Next();
        }

        public IChoice GetChoice (int index) {
            return null;
        }
    }
}
