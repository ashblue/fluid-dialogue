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
        Dictionary<NodeDataBase, NodeDisplayBase> DataToNode { get; }
    }

    public partial class DialogueWindow : EditorWindow, IDialogueWindow {
        private readonly List<NodeDisplayBase> _graveyard = new List<NodeDisplayBase>();

        private DialogueGraph _graph;
        private InputController _mouseEvents;

        private bool IsGraphPopulated => Nodes != null;
        private bool NodesOutOfSync => Nodes.Count != _graph.Nodes.Count;
        public List<NodeDisplayBase> Nodes { get; private set; }
        public Dictionary<NodeDataBase, NodeDisplayBase> DataToNode { get; } =
            new Dictionary<NodeDataBase, NodeDisplayBase>();

        public static void ShowGraph (DialogueGraph graph) {
            var window = GetWindow<DialogueWindow>(false);
            window.titleContent = new GUIContent("Dialogue");
            window.SetGraph(graph);
        }

        private void SetGraph (DialogueGraph graph) {
            BuildNodes(graph);

            _graph = graph;
            _mouseEvents = new InputController(this);
        }

        private void BuildNodes (DialogueGraph graph) {
            DataToNode.Clear();

            Nodes = graph.Nodes
                .Select(CreateNodeInstance)
                .ToList();

            BuildNodeConnections();
        }

        private void BuildNodeConnections () {
            foreach (var node in Nodes) {
                node.Out.Links.RebuildLinks();
            }
        }

        private NodeDisplayBase CreateNodeInstance (NodeDataBase data) {
            var displayType = NodeAssemblies.DataToDisplay[data.GetType()];
            var instance = Activator.CreateInstance(displayType) as NodeDisplayBase;
            if (instance == null) throw new NullReferenceException($"No type found for ${data}");

            instance.Setup(this, data);
            DataToNode[data] = instance;

            return instance;
        }

        private void OnGUI () {
            if (_graph == null) return;

            if (!IsGraphPopulated) {
                SetGraph(_graph);
            }

            if (NodesOutOfSync) {
                BuildNodes(_graph);
            }

            GUI.Label(new Rect(10, 10, 300, 100), $"Dialogue: {_graph.name}", EditorStyles.boldLabel);

            var e = Event.current;
            _mouseEvents.Scroll.UpdateScrollView(position);
            _mouseEvents.ProcessCanvasEvent(e);

            foreach (var node in Nodes) {
                if (node.IsMemoryLeak) {
                    _graveyard.Add(node);
                    continue;
                }

                node.Print();
            }

            _mouseEvents.PaintSelection();

            GUI.EndScrollView();
            UpdateGraveyard();
        }

        private void UpdateGraveyard () {
            foreach (var item in _graveyard) {
                Nodes.Remove(item);
            }

            _graveyard.Clear();
        }

        public void CreateData (NodeDataBase data, Vector2 position) {
            Undo.SetCurrentGroupName("Create node");
            Undo.RecordObject(_graph, "New node");

            NewNode(data, position);

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        private void NewNode (NodeDataBase data, Vector2 position) {
            data.rect.position = position;
            _graph.AddNode(data);
            AssetDatabase.AddObjectToAsset(data, _graph);
            AssetDatabase.SaveAssets();

            var instance = CreateNodeInstance(data);
            Nodes.Add(instance);

            Undo.RegisterCreatedObjectUndo(data, "Create node");
        }

        public void DeleteNode (NodeDisplayBase node) {
            Undo.SetCurrentGroupName("Delete node");
            Undo.RecordObject(_graph, "Delete node");

            CleanupNode(node);

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        public void DeleteNode (IEnumerable<NodeDisplayBase> nodes) {
            Undo.SetCurrentGroupName("Delete nodes");
            Undo.RecordObject(_graph, "Delete node");

            foreach (var node in nodes) {
                CleanupNode(node);
            }

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        private void CleanupNode (NodeDisplayBase node) {
            node.CleanConnections();
            _graph.DeleteNode(node.Data);
            _graveyard.Add(node);
            Undo.DestroyObjectImmediate(node.Data);
        }

        public void DuplicateNode (NodeDisplayBase node) {
            Undo.SetCurrentGroupName("Duplicate node");
            Undo.RecordObject(_graph, "New node");

            var copy = Instantiate(node.Data);
            NewNode(copy, new Vector2(copy.rect.position.x + 50, copy.rect.position.y + 50));

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        public void DuplicateNode (List<NodeDisplayBase> nodes) {
            Undo.SetCurrentGroupName("Duplicate all nodes");
            Undo.RecordObject(_graph, "New node");

            foreach (var node in nodes) {
                var copy = Instantiate(node.Data);
                NewNode(copy, new Vector2(copy.rect.position.x + 50, copy.rect.position.y + 50));
            }

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }
    }
}
