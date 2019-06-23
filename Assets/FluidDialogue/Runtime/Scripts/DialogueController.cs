using System;
using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Graphs;

namespace CleverCrow.Fluid.Dialogues {
    public interface IDialogueController {
        void PlayChild (IGraphData graph);
    }

    public class DialogueController : IDialogueController {
        private readonly Stack<IDialoguePlayback> _activeDialogue = new Stack<IDialoguePlayback>();

        public IDialogueEvents Events { get; } = new DialogueEvents();
        public IDialoguePlayback ActiveDialogue => _activeDialogue.Count > 0 ? _activeDialogue.Peek() : null;

        public void Play (IDialoguePlayback playback) {
            Stop();

            playback.Events.Speak.AddListener(TriggerSpeak);
            playback.Events.Choice.AddListener(TriggerChoice);
            playback.Events.Begin.AddListener(TriggerBegin);
            playback.Events.End.AddListener(TriggerEnd);

            _activeDialogue.Push(playback);
            playback.Play();
        }

        public void Play (IGraphData graph) {
            var runtime = new GraphRuntime(graph);
            Play(new DialoguePlayback(runtime, this, new DialogueEvents()));
        }

        public void PlayChild (IDialoguePlayback playback) {
            if (ActiveDialogue == null) {
                throw new InvalidOperationException("Cannot trigger child dialogue, nothing is playing");
            }

            var parentDialogue = ActiveDialogue;
            playback.Events.End.AddListener(() => {
                _activeDialogue.Pop();
                parentDialogue.Next();
            });
            playback.Events.Speak.AddListener(TriggerSpeak);
            playback.Events.Choice.AddListener(TriggerChoice);

            _activeDialogue.Push(playback);
            playback.Play();
        }

        public void PlayChild (IGraphData graph) {
            var runtime = new GraphRuntime(graph);
            PlayChild(new DialoguePlayback(runtime, this, new DialogueEvents()));
        }

        private void TriggerBegin () {
            Events.Begin.Invoke();
        }

        private void TriggerEnd () {
            _activeDialogue.Pop();
            Events.End.Invoke();
        }

        private void TriggerSpeak (IActor actor, string text) {
            Events.Speak.Invoke(actor, text);
        }

        private void TriggerChoice (IActor actor, string text, List<IChoice> choices) {
            Events.Choice.Invoke(actor, text, choices);
        }

        public void Next () {
            ActiveDialogue?.Next();
        }

        public void Tick () {
            ActiveDialogue?.Tick();
        }

        public void SelectChoice (int index) {
            ActiveDialogue?.SelectChoice(index);
        }

        public void Stop () {
            foreach (var dialogue in _activeDialogue) {
                dialogue.Stop();
            }

            _activeDialogue.Clear();
        }
    }
}
