using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Graphs;

namespace CleverCrow.Fluid.Dialogues {
    public class DialoguePlayback {
        private bool _playing;
        private readonly Queue<IAction> _actionQueue = new Queue<IAction>();

        public IDialogueEvents Events { get;}
        public IDialogueNode Pointer { get; private set; }

        public DialoguePlayback (IDialogueEvents events) {
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

            if (UpdateActionQueue() == ActionStatus.Continue) return;
            Next();
        }

        private void ClearAllActions () {
            while (_actionQueue.Count > 0) {
                var action = _actionQueue.Dequeue();
                action.End();
            }
        }

        private ActionStatus UpdateActionQueue () {
            while (_actionQueue.Count > 0) {
                if (_actionQueue.Peek().Tick() == ActionStatus.Continue) return ActionStatus.Continue;
                _actionQueue.Dequeue();
            }

            return ActionStatus.Success;
        }

        public void Next () {
            if (_actionQueue.Count != 0) return;
            var current = Pointer;
            var next = Pointer.Next();
            Pointer = next;

            Next(current, next);
        }

        private void Next (IDialogueNode current, IDialogueNode next) {
            foreach (var action in current.ExitActions) {
                _actionQueue.Enqueue(action);
            }

            if (next != null) {
                foreach (var action in next.EnterActions) {
                    _actionQueue.Enqueue(action);
                }
            }

            if (UpdateActionQueue() == ActionStatus.Continue) return;
            UpdatePointer(next);
        }

        private void UpdatePointer (IDialogueNode pointer) {
            if (pointer == null) {
                Events.End.Invoke();
                _playing = false;
                return;
            }

            var choices = pointer.GetChoices();
            if (choices.Count > 0) {
                Events.Choice.Invoke(pointer.Actor, pointer.Dialogue, choices);
                return;
            }

            Events.Speak.Invoke(pointer.Actor, pointer.Dialogue);
        }

        public void Tick () {
            if (_actionQueue.Count > 0 && UpdateActionQueue() == ActionStatus.Success) {
                UpdatePointer(Pointer);
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

        public void SelectChoice (int index) {
            var choice = Pointer.GetChoices()[index];
            var current = Pointer;
            Pointer = choice.Node;
            Next(current, choice.Node);
        }
    }
}
