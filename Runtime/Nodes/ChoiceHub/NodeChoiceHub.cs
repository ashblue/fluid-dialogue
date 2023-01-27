using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Conditions;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeChoiceHub : INode {
        private readonly List<IChoice> _choiceList;
        private List<ICondition> _conditions;

        public string UniqueId { get; }
        public List<IAction> EnterActions { get; }
        public List<IAction> ExitActions { get; }

        public virtual bool IsValid =>
            _conditions.Find(c => !c.GetIsValid(this)) == null;

        public List<IChoice> HubChoices =>
            _choiceList.Where(c => c.IsValid).ToList();

        public NodeChoiceHub (string uniqueId, List<IChoice> choiceList, List<ICondition> conditions) {
            UniqueId = uniqueId;
            _choiceList = choiceList;
            _conditions = conditions;
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
