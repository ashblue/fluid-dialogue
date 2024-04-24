using System.Linq;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class NodeBoxStyle {
        private GUIStyle _style;
        private readonly Color _borderColor;
        private readonly Color _backgroundColor;

        private Texture2D _texture;

        public GUIStyle Style {
            get {
                // Unity hammers the texture on play then stop, rebuild if it vanishes
                if (_texture == null) {
                    CreateTexture();
                }

                if (_style == null) {
                    _style = new GUIStyle(GUI.skin.box) {
                        border = new RectOffset(3, 3, 2, 2),
                    };
                }

                if (_style.normal.background == GUI.skin.box.normal.background
                    || _style.normal.background == null) {
                    _style.normal.background = _texture;
                }

                return _style;
            }
        }

        public NodeBoxStyle (Color32 border, Color background) {
            _borderColor = border;
            _backgroundColor = background;

            CreateTexture();
        }

        private void CreateTexture () {
            _texture = new Texture2D(19, 19, TextureFormat.ARGB32, false);
            _texture.filterMode = FilterMode.Point;

            // Create an array for border colors with semi-transparency
            Color[] borderColors = Enumerable.Repeat(new Color(_borderColor.r, _borderColor.g, _borderColor.b, 128), 19 * 19).ToArray();
            _texture.SetPixels(borderColors);

            // Set the internal area to a transparent background color
            Color[] backgroundColors = Enumerable.Repeat(new Color(_backgroundColor.r, _backgroundColor.g, _backgroundColor.b, 0.5f), 17 * 17).ToArray();
            _texture.SetPixels(1, 1, 17, 17, backgroundColors);

            _texture.Apply();
        }
    }
}
