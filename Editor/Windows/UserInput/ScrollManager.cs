using CleverCrow.Fluid.Dialogues.Graphs;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class ScrollManager {
        public const float WINDOW_SIZE = 100000;

        private readonly EditorWindow _window;
        private readonly DialogueGraph _graph;

        public Vector2 ScrollPos {
            get => _graph.scrollPosition;
            set => _graph.scrollPosition = value;
        }

        public Rect ScrollRect { get; private set; }

        public ScrollManager (EditorWindow window, DialogueGraph graph) {
            _graph = graph;
            _window = window;
        }

        public void UpdateScrollView (Rect position) {
            ScrollPos = GUI.BeginScrollView(
                new Rect(0, 0, position.width, position.height),
                ScrollPos,
                new Rect(0, 0, WINDOW_SIZE, WINDOW_SIZE));

            ScrollRect = new Rect(ScrollPos, position.size);
        }

        public void SetViewToRect (Rect rect) {
            var offsetPosition = rect.position;
            offsetPosition.x += rect.width / 2 - _window.position.width / 2;
            offsetPosition.y += rect.height / 2 - _window.position.height / 2;

            ScrollPos = offsetPosition;
        }
    }
}
