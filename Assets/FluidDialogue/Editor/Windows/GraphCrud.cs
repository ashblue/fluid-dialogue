using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Editors.NodeDisplays;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class GraphCrud {
        private readonly DialogueGraph _graph;
        private readonly DialogueWindow _window;

        public GraphCrud (DialogueGraph graph, DialogueWindow window) {
            _graph = graph;
            _window = window;
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

            var instance = _window.CreateNodeInstance(data);
            _window.Nodes.Add(instance);

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
            _window.GraveyardAdd(node);
            Undo.DestroyObjectImmediate(node.Data);
        }

        public void DuplicateNode (NodeDisplayBase node) {
            Undo.SetCurrentGroupName("Duplicate node");
            Undo.RecordObject(_graph, "New node");

            var copy = Object.Instantiate(node.Data);
            NewNode(copy, new Vector2(copy.rect.position.x + 50, copy.rect.position.y + 50));

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        public void DuplicateNode (List<NodeDisplayBase> nodes) {
            Undo.SetCurrentGroupName("Duplicate all nodes");
            Undo.RecordObject(_graph, "New node");

            foreach (var node in nodes) {
                var copy = Object.Instantiate(node.Data);
                NewNode(copy, new Vector2(copy.rect.position.x + 50, copy.rect.position.y + 50));
            }

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }
    }
}
