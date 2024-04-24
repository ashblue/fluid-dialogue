using System;
using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes;
using CleverCrow.Fluid.Dialogues.Nodes.PlayGraph;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues {
    public interface IDialogueController {
        IDatabaseInstance LocalDatabase { get; }

        void PlayChild (IGraphData graph);
    }

    public class DialogueController : IDialogueController {
        private readonly Stack<IDialoguePlayback> _activeDialogue = new Stack<IDialoguePlayback>();
        private readonly List<string> _parentHierarchy = new();

        public IDatabaseInstance LocalDatabase { get; }
        public IDialogueEvents Events { get; } = new DialogueEvents();

        /// <summary>
        /// Playback runtime that corresponds to the current graph definition (may be nested)
        /// </summary>
        public IDialoguePlayback ActiveDialogue => _activeDialogue.Count > 0 ? _activeDialogue.Peek() : null;

        /// <summary>
        /// Return the root of the currently playing dialogue
        /// </summary>
        public IGraphData RootGraph => _activeDialogue.Count > 0 ? _activeDialogue.ToArray().Last().Graph.Data : null;

        /// <summary>
        /// Keeps track of the IDs of all nested graph node containers currently playing in order. Important for restoring nested
        /// dialogue graphs that are currently playing from serialized data.
        /// </summary>
        public IReadOnlyList<string> ParentHierarchy => _parentHierarchy;


        public DialogueController (IDatabaseInstance localDatabase) {
            LocalDatabase = localDatabase;
        }

        public void Play (IDialoguePlayback playback) {
            PlaybackSetup(playback);
            playback.Play();
        }

        // @NOTE gameObjectOverrides will be deprecated. It can easily be replaced with a send message system that looks up the target GameObject by string. This is a lot to maintain and messy
        public void Play (IGraphData graph) {
            var runtime = new GraphRuntime(this, graph);
            Play(new DialoguePlayback(runtime, this, new DialogueEvents()));
        }

        // @TODO Needs some tests
        public void Play (IGraphData graph, IReadOnlyList<string> parentHierarchy, string nodeId) {
            var runtime = new GraphRuntime(this, graph);

            // Setup the root graph with all proper hooks
            var origin = new DialoguePlayback(runtime, this, new DialogueEvents());
            PlaybackSetup(origin);

            var playback = origin;
            foreach (var id in parentHierarchy) {
                // Set and get the pointer
                playback.SetPointer(id);
                if (playback.Pointer is not NodePlayGraph node) {
                    throw new InvalidOperationException($"Parent hierarchy ID {id} does not contain a nested graph");
                }

                // Add the child playback runtime
                AddChild(node.Graph as DialogueGraph);
                playback = _activeDialogue.Peek() as DialoguePlayback;
            }

            playback.SetPointerAndPlay(nodeId);
        }

        void PlaybackSetup (IDialoguePlayback playback) {
            SetupDatabases();

            Stop();

            playback.Events.Speak.AddListener(TriggerSpeak);
            playback.Events.SpeakWithAudio.AddListener(TriggerSpeakWithAudio);
            playback.Events.Choice.AddListener(TriggerChoice);
            playback.Events.NodeEnter.AddListener(TriggerEnterNode);
            playback.Events.Begin.AddListener(TriggerBegin);
            playback.Events.End.AddListener(TriggerEnd);

            _activeDialogue.Push(playback);
        }

        private void SetupDatabases () {
            LocalDatabase.Clear();
        }

        public void PlayChild (IDialoguePlayback playback) {
            SetupChild(playback);
            playback.Play();
        }

        public void PlayChild (IGraphData graph) {
            // @TODO Test this
            _parentHierarchy.Add(_activeDialogue.Peek().Pointer.UniqueId);

            var runtime = new GraphRuntime(this, graph);
            PlayChild(new DialoguePlayback(runtime, this, new DialogueEvents()));
        }

        void AddChild (IGraphData graph) {
            _parentHierarchy.Add(_activeDialogue.Peek().Pointer.UniqueId);

            var runtime = new GraphRuntime(this, graph);
            SetupChild(new DialoguePlayback(runtime, this, new DialogueEvents()));
        }

        void SetupChild (IDialoguePlayback playback) {
            if (ActiveDialogue == null) {
                throw new InvalidOperationException("Cannot trigger child dialogue, nothing is playing");
            }

            var parentDialogue = ActiveDialogue;
            playback.Events.End.AddListener(() => {
                _activeDialogue.Pop();
                parentDialogue.Next();
            });
            playback.Events.Speak.AddListener(TriggerSpeak);
            playback.Events.SpeakWithAudio.AddListener(TriggerSpeakWithAudio);
            playback.Events.Choice.AddListener(TriggerChoice);
            playback.Events.NodeEnter.AddListener(TriggerEnterNode);

            _activeDialogue.Push(playback);
        }

        private void TriggerBegin () {
            Events.Begin.Invoke();
        }

        private void TriggerEnd () {
            _activeDialogue.Pop();
            Events.End.Invoke();
        }

        private void TriggerSpeakWithAudio (IActor actor, string text, AudioClip audioClip) {
            Events.SpeakWithAudio.Invoke(actor, text, audioClip);
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
            // @TODO Test this
            _parentHierarchy.Clear();
        }

        // @TODO Write a test for this if possible, might not be easy
        /// <summary>
        /// Verifies a nested graph can be played. Not the most runtime friendly operation so use sparingly
        /// </summary>
        public bool CanPlay (IGraphData graph, List<string> parentHierarchy, string nodeId) {
            // Use the parent hierarchy to find the nested graph
            var nestedGraph = graph;
            foreach (var id in parentHierarchy) {
                var node = GetNode(nestedGraph, id);
                if (node == null) return false;

                if (node is NodePlayGraphData playGraph) {
                    nestedGraph = playGraph.dialogueGraph;
                } else {
                    return false;
                }
            }

            // Check if the node exists in the nested graph
            return GetNode(nestedGraph, nodeId) != null;
        }

        INodeData GetNode (IGraphData graph, string id) {
            foreach (var node in graph.Nodes) {
                if (node.UniqueId == id) {
                    return node;
                }
            }

            return null;
        }
    }
}
