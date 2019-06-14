using CleverCrow.Fluid.Dialogues.Graphs;

namespace CleverCrow.Fluid.Dialogues {
    public class DialoguePlayback {
        private IDialogueNode _pointer;

        public IDialoguePlaybackEvents Events { get;}

        public DialoguePlayback (IDialoguePlaybackEvents events) {
            Events = events;
        }

        public void Play (IDialogueGraph graph) {
            var activeNode = graph.Root.Next();
            if (activeNode == null) return;

            _pointer = activeNode;
            Events.Begin.Invoke();
            Events.Speak.Invoke(activeNode.Actor, activeNode.Dialogue);
        }

        public void Next () {
            _pointer = _pointer.Next();

            if (_pointer == null) {
                Events.End.Invoke();
                return;
            }

            Events.Speak.Invoke(_pointer.Actor, _pointer.Dialogue);
        }
    }
}
