using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Graphs;

namespace CleverCrow.Fluid.Dialogues {
    public class DialoguePlayback {
        private Queue<IAction> _actionQueue = new Queue<IAction>();
        private IDialogueNode _pointer;

        public IDialoguePlaybackEvents Events { get;}

        public DialoguePlayback (IDialoguePlaybackEvents events) {
            Events = events;
        }

        public void Play (IDialogueGraph graph) {
            // @TODO Trigger root enter actions and set pointer to root

            // @TODO Call Next() instead of running this
            var activeNode = graph.Root.Next();
            if (activeNode == null) return;

            _pointer = activeNode;
            Events.Begin.Invoke();
            Events.Speak.Invoke(activeNode.Actor, activeNode.Dialogue);
        }

        public void Next () {
            if (_actionQueue.Count != 0) return;

            foreach (var action in _pointer.ExitActions) {
                _actionQueue.Enqueue(action);
            }

            foreach (var action in _pointer.EnterActions) {
                _actionQueue.Enqueue(action);
            }

            if (!Tick()) return;
            AdvancePointer();
        }

        private void AdvancePointer () {
            _pointer = _pointer.Next();

            if (_pointer == null) {
                Events.End.Invoke();
                return;
            }

            Events.Speak.Invoke(_pointer.Actor, _pointer.Dialogue);
        }

        public bool Tick () {
            while (_actionQueue.Count > 0) {
                if (!_actionQueue.Peek().Tick()) return false;
                _actionQueue.Dequeue();

                if (_actionQueue.Count == 0) AdvancePointer();
            }

            return true;
        }
    }
}
