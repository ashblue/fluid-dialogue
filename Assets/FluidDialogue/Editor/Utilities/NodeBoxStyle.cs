using System.Linq;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class NodeBoxStyle {
        private readonly GUIStyle _style;
        private readonly Color32 _borderColor;
        private readonly Color _backgroundColor;

        private Texture2D _texture;

        public GUIStyle Style {
            get {
                // Unity hammers the texture on play then stop, rebuild if it vanishes
                if (_texture == null) {
                    CreateTexture();
                    _style.normal.background = _texture;
                }

                return _style;
            }
        }

        public NodeBoxStyle (Color32 border, Color background) {
            _borderColor = border;
            _backgroundColor = background;

            CreateTexture();

            _style = new GUIStyle(GUI.skin.box) {
                border = new RectOffset(1, 1, 1, 1),
                normal = {
                    background = _texture,
                },
            };
        }

        private void CreateTexture () {
            _texture = Texture2DExtensions.CreateTexture(19, 19, _borderColor);
            _texture.SetPixels(1, 1, 17, 17,
                Enumerable.Repeat(_backgroundColor, 17 * 17).ToArray());
            _texture.Apply();
        }
    }
}
