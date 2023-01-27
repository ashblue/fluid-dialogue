using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class FluidDialogueSettings : ScriptableObject {
        private const string ASSET_PATH = "Assets/Resources/FluidDialogueSettings.asset";

        [Tooltip("Prevents meta data like choices, actions, and conditions from being set to visible in the " +
                 "nested object Project window view. Keeping set to true is highly recommended.")]
        [SerializeField]
        private bool _hideNestedNodeData = true;

        public static FluidDialogueSettings Current => GetOrCreateSettings();

        public bool HideNestedNodeData => _hideNestedNodeData;

        private static FluidDialogueSettings GetOrCreateSettings () {
            var settings = AssetDatabase.LoadAssetAtPath<FluidDialogueSettings>(ASSET_PATH);
            if (AssetDatabase.LoadAssetAtPath<Object>("Assets/Resources") == null)
                AssetDatabase.CreateFolder("Assets", "Resources");
            if (settings != null) return settings;

            settings = CreateInstance<FluidDialogueSettings>();
            AssetDatabase.CreateAsset(settings, ASSET_PATH);
            AssetDatabase.SaveAssets();

            return settings;
        }

        internal static SerializedObject GetSerializedSettings () {
            return new SerializedObject(GetOrCreateSettings());
        }
    }

    static class FluidDialogueSettingsIMGUIRegister {
        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider () {
            // First parameter is the path in the Settings window.
            // Second parameter is the scope of this setting: it only appears in the Project Settings window.
            var provider = new SettingsProvider("Preferences/Node Editor", SettingsScope.User) {
                label = "Fluid Dialogue",
                keywords = new HashSet<string>(new[] {"fluid", "dialogue"}),
                guiHandler = (searchContext) => {
                    var settings = FluidDialogueSettings.GetSerializedSettings();
                    EditorGUILayout.PropertyField(settings.FindProperty("_hideNestedNodeData"));
                    settings.ApplyModifiedProperties();
                },
            };

            return provider;
        }
    }
}
