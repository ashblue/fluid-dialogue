using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues {
    public interface IDialoguePlayback {
        IDialogueEvents Events { get; }
        IDialogueController ParentCtrl { get; }

        void Next ();
        void Play ();
        void Tick ();
        void SelectChoice (int index);
        void Stop ();
    }

    public class DialoguePlayback : IDialoguePlayback {
        private bool _playing;
        private readonly Queue<IAction> _actionQueue = new Queue<IAction>();
        private readonly IGraph _graph;

        public IDialogueEvents Events { get;}
        public IDialogueController ParentCtrl { get; }
        public INode Pointer { get; private set; }

        public DialoguePlayback (IGraph graph, IDialogueController ctrl, IDialogueEvents events) {
            _graph = graph;
            Events = events;
            ParentCtrl = ctrl;
        }

        public void Play () {
            Stop();

            _playing = true;
            Pointer = _graph.Root;
            Events.Begin.Invoke();

            if (!_graph.Root.IsValid) {
                Events.End.Invoke();
                return;
            }

            Next(null, Pointer);
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

        private void Next (INode current, INode next) {
            if (current != null) {
                foreach (var action in current.ExitActions) {
                    _actionQueue.Enqueue(action);
                }
            }

            if (next != null) {
                foreach (var action in next.EnterActions) {
                    _actionQueue.Enqueue(action);
                }
            }

            if (UpdateActionQueue() == ActionStatus.Continue) return;
            UpdatePointer(next);
        }

        private void UpdatePointer (INode pointer) {
            if (pointer == null) {
                Events.End.Invoke();
                _playing = false;
                return;
            }

            pointer.Play(this);
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
            var choice = Pointer.GetChoice(index);
            var current = Pointer;
            Pointer = choice.GetValidChildNode();
            Next(current, Pointer);
        }
    }
}
