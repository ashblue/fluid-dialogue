using UnityEditor;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public static class ThemeUtility {
        public static bool IsDarkTheme => EditorGUIUtility.isProSkin;
    }
}
