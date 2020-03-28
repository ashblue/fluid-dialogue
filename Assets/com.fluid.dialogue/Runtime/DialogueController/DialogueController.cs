using System;
using System.Collections.Generic;
using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues {
    public interface IDialogueController {
        IDatabaseInstance LocalDatabase { get; }
        IDatabaseInstanceExtended LocalDatabaseExtended { get; }

        void PlayChild (IGraphData graph);
    }

    public class DialogueController : IDialogueController {
        private readonly Stack<IDialoguePlayback> _activeDialogue = new Stack<IDialoguePlayback>();

        [Obsolete("Use LocalDatabaseExtended instead")]
        public IDatabaseInstance LocalDatabase { get; }
        public IDatabaseInstanceExtended LocalDatabaseExtended { get; }
        public IDialogueEvents Events { get; } = new DialogueEvents();
        public IDialoguePlayback ActiveDialogue => _activeDialogue.Count > 0 ? _activeDialogue.Peek() : null;

        [Obsolete("Use DatabaseInstanceExtended instead. Old databases do not support GameObjects")]
        public DialogueController (IDatabaseInstance localDatabase) {
            LocalDatabase = localDatabase;
        }

        public DialogueController (IDatabaseInstanceExtended localDatabase) {
#pragma warning disable 618
            LocalDatabase = localDatabase;
#pragma warning restore 618

            LocalDatabaseExtended = localDatabase;
        }

        public void Play (IDialoguePlayback playback, IGameObjectOverride[] gameObjectOverrides = null) {
            SetupDatabases(gameObjectOverrides);

            Stop();

            playback.Events.Speak.AddListener(TriggerSpeak);
            playback.Events.Choice.AddListener(TriggerChoice);
            playback.Events.NodeEnter.AddListener(TriggerEnterNode);
            playback.Events.Begin.AddListener(TriggerBegin);
            playback.Events.End.AddListener(TriggerEnd);

            _activeDialogue.Push(playback);
            playback.Play();
        }

        public void Play (IGraphData graph, IGameObjectOverride[] gameObjectOverrides = null) {
            var runtime = new GraphRuntime(this, graph);
            Play(new DialoguePlayback(runtime, this, new DialogueEvents()), gameObjectOverrides);
        }

        private void SetupDatabases (IGameObjectOverride[] gameObjectOverrides) {
#pragma warning disable 618
            LocalDatabase.Clear();
#pragma warning restore 618

            if (LocalDatabaseExtended == null) return;
            LocalDatabaseExtended.ClearGameObjects();

            if (gameObjectOverrides == null) return;
            foreach (var goOverride in gameObjectOverrides) {
                LocalDatabaseExtended.GameObjects.Set(goOverride.Definition.Key, goOverride.Value);
            }
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
            playback.Events.NodeEnter.AddListener(TriggerEnterNode);

            _activeDialogue.Push(playback);
            playback.Play();
        }

        public void PlayChild (IGraphData graph) {
            var runtime = new GraphRuntime(this, graph);
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

        private void TriggerEnterNode (INode node) {
            Events.NodeEnter.Invoke(node);
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
