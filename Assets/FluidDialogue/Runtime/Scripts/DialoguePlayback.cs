using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Graphs;

namespace CleverCrow.Fluid.Dialogues {
    public class DialoguePlayback {
        private bool _playing;
        private readonly Queue<IAction> _actionQueue = new Queue<IAction>();

        public IDialoguePlaybackEvents Events { get;}
        public IDialogueNode Pointer { get; private set; }

        public DialoguePlayback (IDialoguePlaybackEvents events) {
            Events = events;
        }

        public void Play (IDialogueGraph graph) {
            Stop();

            _playing = true;
            Pointer = graph.Root;
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

            foreach (var action in Pointer.ExitActions) {
                _actionQueue.Enqueue(action);
            }

            Pointer = Pointer.Next();
            if (Pointer != null) {
                foreach (var action in Pointer.EnterActions) {
                    _actionQueue.Enqueue(action);
                }
            }

            if (!UpdateActionQueue()) return;
            UpdatePointer();
        }

        private void UpdatePointer () {
            if (Pointer == null) {
                Events.End.Invoke();
                _playing = false;
                return;
            }

            Events.Speak.Invoke(Pointer.Actor, Pointer.Dialogue);
        }

        public void Tick () {
            if (_actionQueue.Count > 0 && UpdateActionQueue()) {
                UpdatePointer();
            }
        }

        public void Stop () {
            Pointer = null;
            ClearAllActions();

            if (_playing) {
                Events.End.Invoke();
                _playing = false;
            }
        }
    }
}
