using System;
using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Editors.NodeDisplays;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public partial class DialogueWindow : EditorWindow {
        private readonly List<NodeDisplayBase> _graveyard = new List<NodeDisplayBase>();
        private DialogueGraph _graph;
        private InputController _mouseEvents;

        private bool IsGraphPopulated => Nodes != null;
        private bool NodesOutOfSync => Nodes.Count != _graph.Nodes.Count;
        public List<NodeDisplayBase> Nodes { get; private set; }

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
            Nodes = graph.Nodes
                .Select(CreateNodeInstance)
                .ToList();
        }

        private NodeDisplayBase CreateNodeInstance (NodeDataBase data) {
            var displayType = NodeAssemblies.DataToDisplay[data.GetType()];
            var instance = Activator.CreateInstance(displayType) as NodeDisplayBase;
            if (instance == null) throw new NullReferenceException($"No type found for ${data}");
            instance.Setup(this, data);
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

            data.rect.position = position;
            _graph.AddNode(data);
            AssetDatabase.AddObjectToAsset(data, _graph);
            AssetDatabase.SaveAssets();

            var instance = CreateNodeInstance(data);
            Nodes.Add(instance);

            Undo.RegisterCreatedObjectUndo(data, "Create node");
            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        public void DeleteNode (NodeDisplayBase node) {
            Undo.SetCurrentGroupName("Delete node");
            Undo.RecordObject(_graph, "Delete node");

            _graph.DeleteNode(node.Data);
            _graveyard.Add(node);

            Undo.DestroyObjectImmediate(node.Data);
            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        public void DeleteNode (IEnumerable<NodeDisplayBase> nodes) {
            Undo.SetCurrentGroupName("Delete nodes");
            Undo.RecordObject(_graph, "Delete node");

            foreach (var node in nodes) {
                _graph.DeleteNode(node.Data);
                _graveyard.Add(node);

                Undo.DestroyObjectImmediate(node.Data);
            }

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }
    }
}
