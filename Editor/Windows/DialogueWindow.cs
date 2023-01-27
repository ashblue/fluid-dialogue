using System;
using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Editors.NodeDisplays;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public interface IDialogueWindow {
        Dictionary<NodeDataBase, NodeEditorBase> DataToNode { get; }
    }

    public partial class DialogueWindow : EditorWindow, IDialogueWindow {
        private readonly List<NodeEditorBase> _graveyard = new List<NodeEditorBase>();

        private bool IsGraphPopulated => Nodes != null;
        private bool NodesOutOfSync => Nodes.Count != Graph.Nodes.Count;
        public List<NodeEditorBase> Nodes { get; private set; }
        public GraphCrud GraphCrud { get; private set; }
        public InputController MouseEvents { get; private set; }

        public Dictionary<NodeDataBase, NodeEditorBase> DataToNode { get; } =
            new Dictionary<NodeDataBase, NodeEditorBase>();

        private static DialogueGraph GraphData { get; set; }
        public DialogueGraph Graph => GraphData;

        public static void ShowGraph (DialogueGraph graph, bool focus = true) {
            var window = GetWindow<DialogueWindow>(false, "Dialogue", focus);
            window.SetGraph(graph);
        }

        public static void SaveGraph () {
            if (GraphData == null) return;
            EditorUtility.SetDirty(GraphData);
            AssetDatabase.SaveAssets();
        }

        private void SetGraph (DialogueGraph graph) {
            GraphCrud = new GraphCrud(graph, this);
            BuildNodes(graph);

            GraphData = graph;
            MouseEvents = new InputController(this);
            SaveGraphToEditor(graph);
        }

        private void BuildNodes (DialogueGraph graph) {
            DataToNode.Clear();

            Nodes = graph.Nodes
                .Select(i => CreateNodeInstance(i as NodeDataBase))
                .ToList();

            BuildNodeConnections();
        }

        private void BuildNodeConnections () {
            foreach (var node in Nodes) {
                foreach (var connection in node.Out) {
                    connection.Links.RebuildLinks();
                }
            }
        }

        public NodeEditorBase CreateNodeInstance (NodeDataBase data) {
            var displayType = NodeAssemblies.DataToDisplay[data.GetType()];
            var instance = Activator.CreateInstance(displayType) as NodeEditorBase;
            if (instance == null) throw new NullReferenceException($"No type found for ${data}");

            instance.Setup(this, data);
            DataToNode[data] = instance;

            return instance;
        }

        private void OnGUI () {
            if (Graph == null) return;

            if (!IsGraphPopulated) {
                SetGraph(Graph);
            }

            if (NodesOutOfSync) {
                BuildNodes(Graph);
            }

            GUI.Label(new Rect(10, 10, 300, 100), $"Dialogue: {Graph.name}", EditorStyles.boldLabel);

            var e = Event.current;
            MouseEvents.Scroll.UpdateScrollView(position);
            MouseEvents.ProcessCanvasEvent(e);

            foreach (var node in Nodes) {
                if (node.IsMemoryLeak) {
                    GraveyardAdd(node);
                    continue;
                }

                node.Print();
            }

            MouseEvents.PaintSelection();

            GUI.EndScrollView();
            UpdateGraveyard();
        }

        private void UpdateGraveyard () {
            if (_graveyard.Count == 0) return;

            foreach (var item in _graveyard) {
                Nodes.Remove(item);
            }

            _graveyard.Clear();
            AssetDatabase.SaveAssets();
        }

        public void GraveyardAdd (NodeEditorBase node) {
            _graveyard.Add(node);
        }
    }
}
