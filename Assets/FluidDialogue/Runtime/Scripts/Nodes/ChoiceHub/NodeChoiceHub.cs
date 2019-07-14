using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Choices;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeChoiceHub : INode {
        private readonly List<IChoice> _choiceList;

        public string UniqueId { get; }
        public List<IAction> EnterActions { get; }
        public List<IAction> ExitActions { get; }
        public bool IsValid { get; }
        public List<IChoice> HubChoices =>
            _choiceList.Where(c => c.GetValidChildNode() != null).ToList();

        public NodeChoiceHub (string uniqueId, List<IChoice> choiceList) {
            UniqueId = uniqueId;
            _choiceList = choiceList;
        }

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
