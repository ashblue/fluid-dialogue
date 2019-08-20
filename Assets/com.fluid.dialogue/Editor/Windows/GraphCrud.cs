using System.Collections.Generic;
using System.Linq;
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
            data.ClearConnectionChildren();
            data.rect.position = position;
            _graph.AddNode(data);
            AssetDatabase.AddObjectToAsset(data, _graph);
            AssetDatabase.SaveAssets();

            var instance = _window.CreateNodeInstance(data);
            _window.Nodes.Add(instance);

            Undo.RegisterCreatedObjectUndo(data, "Create node");
        }

        public void DeleteNode (NodeEditorBase node) {
            Undo.SetCurrentGroupName("Delete node");
            Undo.RecordObject(_graph, "Delete node");

            CleanupNode(node);

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        public void DeleteNode (IEnumerable<NodeEditorBase> nodes) {
            Undo.SetCurrentGroupName("Delete nodes");
            Undo.RecordObject(_graph, "Delete node");

            foreach (var node in nodes) {
                CleanupNode(node);
            }

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        private void CleanupNode (NodeEditorBase node) {
            Undo.RecordObject(node.Data, "Delete node");

            node.CleanConnections();
            _graph.DeleteNode(node.Data);
            _window.GraveyardAdd(node);
            Undo.DestroyObjectImmediate(node.Data);

            var childObjects = node.Data.enterActions
                .Concat<ScriptableObject>(node.Data.exitActions)
                .Concat(node.Data.conditions);

            foreach (var scriptableObject in childObjects) {
                Undo.DestroyObjectImmediate(scriptableObject);
            }

            node.DeleteCleanup();
        }

        public void DuplicateNode (NodeEditorBase node) {
            Undo.SetCurrentGroupName("Duplicate node");
            Undo.RecordObject(_graph, "New node");

            var copy = node.CreateDataCopy();
            NewNode(copy, new Vector2(copy.rect.position.x + 50, copy.rect.position.y + 50));

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }

        public void DuplicateNode (List<NodeEditorBase> nodes) {
            Undo.SetCurrentGroupName("Duplicate all nodes");
            Undo.RecordObject(_graph, "New node");

            foreach (var node in nodes) {
                var copy = node.CreateDataCopy();
                NewNode(copy, new Vector2(copy.rect.position.x + 50, copy.rect.position.y + 50));
            }

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        }
    }
}
