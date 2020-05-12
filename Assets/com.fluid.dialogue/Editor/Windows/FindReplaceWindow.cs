using System;
using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.FindAndReplace.Editors;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class SearchResult : IFindResult {
        private readonly UnityEngine.Object _target;
        public string Text { get; }

        public SearchResult (string text, UnityEngine.Object target) {
            _target = target;
            Text = text;
        }

        public void Show () {
            Selection.activeObject = _target;;
        }

        public void Replace (int startIndex, int charactersToReplace, string replaceText) {
            Debug.Log("wip");
            // var beginning = Text.Substring(0, startIndex);
            // var end = Text.Substring(startIndex + charactersToReplace);
            // var text = $"{beginning}{replaceText}{end}";
            //
            // var obj = new SerializedObject(_target);
            // obj.FindProperty("text").stringValue = text;
            // obj.ApplyModifiedProperties();
        }
    }

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

                foreach (var node in graph.Nodes) {
                    if (IsValid(node.Text ?? "")) {
                        var result = new SearchResult(node.Text, node as UnityEngine.Object);
                        results.Add(result);
                    }

                    // @TODO Get choice text from each node
                    // @TODO Different SearchResult type to handle choices (for replacing)
                }
            }

            return results.ToArray();
        }
    }
}
