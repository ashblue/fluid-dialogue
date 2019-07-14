using System;
using CleverCrow.Fluid.Dialogues.Graphs;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class DialogueWindow : EditorWindow {
        private DialogueGraph _graph;

        public static void ShowGraph (DialogueGraph graph) {
            var window = GetWindow<DialogueWindow>(false);
            window.SetGraph(graph);
        }

        private void SetGraph (DialogueGraph graph) {
            _graph = graph;
        }

        private void OnGUI () {
            GUI.Label(new Rect(10, 10, 300, 100), $"Dialogue: {_graph?.name}", EditorStyles.boldLabel);
        }
    }
}
