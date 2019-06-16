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
            ClearAllActions();

            _pointer = graph.Root;
            Events.Begin.Invoke();
            foreach (var action in graph.Root.EnterActions) {
                _actionQueue.Enqueue(action);
            }

            if (!UpdateActionQueue()) return;
            Next();
        }

        private void ClearAllActions () {
            while (_actionQueue.Count > 0) {
                var action = _actionQueue.Dequeue();
                action.End();
            }
        }

        private bool UpdateActionQueue () {
            while (_actionQueue.Count > 0) {
                if (!_actionQueue.Peek().Tick()) return false;
                _actionQueue.Dequeue();
            }

            return true;
        }

        public void Next () {
            if (_actionQueue.Count != 0) return;

            foreach (var action in _pointer.ExitActions) {
                _actionQueue.Enqueue(action);
            }

            _pointer = _pointer.Next();
            if (_pointer != null) {
                foreach (var action in _pointer.EnterActions) {
                    _actionQueue.Enqueue(action);
                }
            }

            if (!UpdateActionQueue()) return;
            UpdatePointer();
        }

        private void UpdatePointer () {
            if (_pointer == null) {
                Events.End.Invoke();
                return;
            }

            Events.Speak.Invoke(_pointer.Actor, _pointer.Dialogue);
        }

        public void Tick () {
            if (_actionQueue.Count > 0 && UpdateActionQueue()) {
                UpdatePointer();
            }
        }
    }
}
