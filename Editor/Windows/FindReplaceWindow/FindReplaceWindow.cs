using System;
using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.FindAndReplace.Editors;
using UnityEditor;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class FindReplaceWindow : FindReplaceWindowBase {
        [MenuItem("Window/Fluid Dialogue/Find Replace")]
        public static void ShowFindReplace () {
            ShowWindow<FindReplaceWindow>();
        }

        protected override IFindResult[] GetFindResults (Func<string, bool> IsValid) {
            var results = new List<IFindResult>();

            var graphIDs = AssetDatabase.FindAssets("t:DialogueGraph");
            foreach (var graphID in graphIDs) {
                var path = AssetDatabase.GUIDToAssetPath(graphID);
                var graph = AssetDatabase.LoadAssetAtPath<DialogueGraph>(path);
                if (graph == null) continue;

                foreach (var node in graph.Nodes) {
                    if (IsValid(node.Text ?? "")) {
                        var result = new DialogueSearchResult(node.Text, node as UnityEngine.Object);
                        results.Add(result);
                    }

                    foreach (var choice in node.Choices) {
                        var result = new ChoiceSearchResult(choice.text, choice);
                        results.Add(result);
                    }
                }
            }

            return results.ToArray();
        }
    }
}
