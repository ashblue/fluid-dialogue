using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Actions;

namespace CleverCrow.Fluid.Dialogues.Nodes {
//    public class NodeDialogueOld : INodeRuntime {
//        public override List<IAction> ExitActions { get; }
//        public override List<IAction> EnterActions { get; }
//        public override bool IsValid { get; }
//
//        public void Setup () {
//            _internal = new DialogueNodeInternal();
//        }
//
//        public override INodeRuntime Clone () {
//            var clone = Instantiate(this);
//            clone.Setup();
//
//            return clone;
//        }
//
//        public override INodeRuntime Next () {
//            return _internal.GetValidChild(_children.ToList<INodeRuntime>());
//        }
//
//        public override void Play (DialoguePlayback playback) {
//            _validChoices = _internal.GetValidChoices(_choices.ToList<IChoiceRuntime>());
//            if (_validChoices.Count > 0) {
//                playback.Events.Choice.Invoke(_actor, _dialogue, _validChoices);
//                return;
//            }
//
//            playback.Events.Speak.Invoke(_actor, _dialogue);
//        }
//
//        public override IChoiceRuntime GetChoice (int index) {
//            return _validChoices[index];
//        }
//    }

    public class NodeDialogue : INodeRuntime {
        private readonly List<INodeRuntime> _childNodes;

        public NodeDialogue (List<INodeRuntime> childNodes) {
            _childNodes = childNodes;
        }

        public List<IChoiceRuntime> GetValidChoices (List<IChoiceRuntime> choices) {
            return choices.Where(c => c.Node.IsValid).ToList();
        }

        public List<IAction> ExitActions { get; }
        public List<IAction> EnterActions { get; }
        public bool IsValid { get; }

        public INodeRuntime Next () {
            return _childNodes.Find(c => c.IsValid);
        }

        public void Play (DialoguePlayback playback) {
            throw new System.NotImplementedException();
        }

        public IChoiceRuntime GetChoice (int index) {
            throw new System.NotImplementedException();
//            return _choices.Where(c => c.Node.IsValid).ToList();
        }
    }
}
