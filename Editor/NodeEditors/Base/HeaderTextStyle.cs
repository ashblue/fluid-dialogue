using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class HeaderTextStyle {
        private GUIStyle _style;
        private bool _init;

        public GUIStyle Style {
            get {
                if (!_init && EditorStyles.centeredGreyMiniLabel != null) {
                    _init = true;
                    _style = EditorStyles.centeredGreyMiniLabel;
                    _style.normal.textColor = Color.black;
                } else if (_style == null) {
                    _style = new GUIStyle();
                }

                return _style;
            }
        }
    }
}
