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
        private List<NodeDisplayBase> _nodes;
        private ViewController _mouseEvents;

        private bool IsGraphPopulated => _nodes != null;
        private bool NodesOutOfSync => _nodes.Count != _graph.Nodes.Count;

        public static void ShowGraph (DialogueGraph graph) {
            var window = GetWindow<DialogueWindow>(false);
            window.titleContent = new GUIContent("Dialogue");
            window.SetGraph(graph);
        }

        private void SetGraph (DialogueGraph graph) {
            BuildNodes(graph);

            _graph = graph;
            _mouseEvents = new ViewController(this);
        }

        private void BuildNodes (DialogueGraph graph) {
            _nodes = graph.Nodes
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
            _mouseEvents.UpdateScrollView(position);

            var nodeSelected = false;
            foreach (var node in _nodes) {
                if (node.IsMemoryLeak) {
                    _graveyard.Add(node);
                    continue;
                }

                node.ProcessEvent(e);
                if (!nodeSelected) nodeSelected = node.IsSelected;
                node.Print();
            }

            if (!nodeSelected) {
                _mouseEvents.ProcessCanvasEvent(e);
            }

            GUI.EndScrollView();
            UpdateGraveyard();
        }

        private void UpdateGraveyard () {
            foreach (var item in _graveyard) {
                _nodes.Remove(item);
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
            _nodes.Add(instance);

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
    }
}
