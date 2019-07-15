using System.Linq;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public static class Texture2DExtensions {
        public static Texture2D CreateTexture (int width, int height, Color color) {
            var texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            texture.SetPixels(Enumerable.Repeat(color, width * height).ToArray());
            texture.Apply();

            return texture;
        }
    }
}
