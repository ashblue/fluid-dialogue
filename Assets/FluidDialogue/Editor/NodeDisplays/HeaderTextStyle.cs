using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class HeaderTextStyle {
        private readonly GUIStyle _style;

        public GUIStyle Style => _style;

        public HeaderTextStyle () {
            _style = EditorStyles.centeredGreyMiniLabel;
            _style.normal.textColor = Color.black;
        }
    }
}
