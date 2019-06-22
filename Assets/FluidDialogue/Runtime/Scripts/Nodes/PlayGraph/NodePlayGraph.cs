using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Choices;

namespace CleverCrow.Fluid.Dialogues.Nodes.PlayGraph {
    public class NodePlayGraph : INode {
        public List<IAction> EnterActions { get; }
        public List<IAction> ExitActions { get; }
        public bool IsValid { get; }
        public List<IChoice> HubChoices { get; }
        public INode Next () {
            throw new System.NotImplementedException();
        }

        public void Play (IDialoguePlayback playback) {
            throw new System.NotImplementedException();
        }

        public IChoice GetChoice (int index) {
            throw new System.NotImplementedException();
        }
    }
}
