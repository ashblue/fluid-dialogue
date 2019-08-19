using CleverCrow.Fluid.Dialogues.Graphs;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.Inspectors {
    [CustomEditor(typeof(DialogueGraph))]
    public class DialogueGraphInspector : Editor {
        public override void OnInspectorGUI () {
            DrawDefaultInspector();

            if (GUILayout.Button("Edit Dialogue")) {
                DialogueWindow.ShowGraph(target as DialogueGraph);
            }
        }
    }
}
