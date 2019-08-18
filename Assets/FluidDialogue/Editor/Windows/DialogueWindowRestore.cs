using CleverCrow.Fluid.Dialogues.Graphs;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public partial class DialogueWindow {
        private const string PREF_ASSET_GUID = "FluidDialogue_CurrentAsset";

        private static bool HasSavedGraphId => EditorPrefs.HasKey(PREF_ASSET_GUID);

        private static void SaveGraphToEditor (DialogueGraph graph) {
            var path = AssetDatabase.GetAssetPath(graph);
            var guid = AssetDatabase.AssetPathToGUID(path);

            EditorPrefs.SetString(PREF_ASSET_GUID, guid);
        }

        private static DialogueGraph GetGraphFromEditor () {
            var guid = EditorPrefs.GetString(PREF_ASSET_GUID);
            var graphPath = AssetDatabase.GUIDToAssetPath(guid);
            if (graphPath == null) {
                EditorPrefs.DeleteKey(PREF_ASSET_GUID);
                return null;
            }

            return AssetDatabase.LoadAssetAtPath<DialogueGraph>(graphPath);
        }

        // @NOTE Save and restore must be done outside of the Window's OnGui loop to prevent crashing layout groups
        [UnityEditor.Callbacks.DidReloadScripts]
        public static void RestoreSavedGraph () {
            if (!EditorPrefs.GetBool(PREF_OPEN, false)) return;
            if (!HasSavedGraphId) return;
            var saveGraph = GetGraphFromEditor();
            if (saveGraph == null) return;
            ShowGraph(saveGraph, false);
        }
    }
}
