using System.Linq;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public static class CreateDialogueGraph {
        [MenuItem("Assets/Create/Fluid/Dialogue/Graph", priority = 0)]
        public static void CreateAsset () {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            CreateAsset(path, "Dialogue");

            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Create a dialogue graph asset in a specific folder.
        /// Designed to create dialogue graphs through custom Unity editor scripts.
        /// You must call AssetDatabase.SaveAssets(); on your own to save the asset properly.
        /// </summary>
        public static DialogueGraph CreateAsset (string folderPath, string graphName) {
            var graph = CreateGraph(folderPath, graphName);

            var root = ScriptableObject.CreateInstance<NodeRootData>();
            root.rect.position =
                new Vector2(50 + ScrollManager.WINDOW_SIZE /2, 200 + ScrollManager.WINDOW_SIZE / 2);
            graph.AddNode(root);
            graph.root = root;

            AssetDatabase.AddObjectToAsset(root, graph);

            return graph;
        }

        private static DialogueGraph CreateGraph (string folderPath, string graphName) {
            var graph = ScriptableObject.CreateInstance<DialogueGraph>();
            graph.name = graphName;
            var assetsInPath = AssetDatabase
                .FindAssets("t:DialogueGraph", new[] {folderPath})
                .Select(i => {
                    var p = AssetDatabase.GUIDToAssetPath(i);
                    var parts = p.Split('/');
                    return parts[parts.Length - 1].Replace(".asset", "");
                })
                .ToList();

            var count = 0;
            while (assetsInPath.Find(i => i == graph.name) != null) {
                count++;
                var name = graph.name.Split('(')[0];
                graph.name = $"{name}({count})";
            }

            AssetDatabase.CreateAsset(graph, $"{folderPath}/{graph.name}.asset");
            return graph;
        }
    }
}
