using System.Linq;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public static class CreateDialogueGraph {
        [MenuItem("Assets/Create/Dialogue Graph", priority = 0)]
        public static void CreateAsset () {
            var graph = CreateGraph();

            var root = ScriptableObject.CreateInstance<NodeRootData>();
            root.name = "Root";
            graph.AddNode(root);
            graph.root = root;
            AssetDatabase.AddObjectToAsset(root, graph);

            AssetDatabase.SaveAssets();
        }

        private static DialogueGraph CreateGraph () {
            var graph = ScriptableObject.CreateInstance<DialogueGraph>();
            graph.name = "Dialogue";
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            var assetsInPath = AssetDatabase
                .FindAssets("t:DialogueGraph", new[] {path})
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

            AssetDatabase.CreateAsset(graph, $"{path}/{graph.name}.asset");
            return graph;
        }
    }
}
