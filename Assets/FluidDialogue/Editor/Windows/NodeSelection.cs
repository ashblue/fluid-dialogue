using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Editors.NodeDisplays;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class NodeSelection {
        private readonly EditorWindow _window;
        private readonly NodeBoxStyle _selectBoxStyle = new NodeBoxStyle(Color.black, new Color(0, 0, 0, 0.5f));

        public Rect area;

        public List<NodeDisplayBase> Selected { get; } = new List<NodeDisplayBase>();

        public NodeSelection (EditorWindow window) {
            _window = window;
        }

        public void Add (NodeDisplayBase node) {
            Selected.Add(node);
            node.Select();
        }

        public void Remove (NodeDisplayBase node) {
            Selected.Remove(node);
            node.Deselect();
        }

        public void PaintSelection () {
            if (!(area.size.magnitude > 1)) return;

            GUI.Box(area, GUIContent.none, _selectBoxStyle.Style);
            _window.Repaint();
        }

        public void RemoveAll () {
            foreach (var node in Selected.ToList()) {
                Remove(node);
            }
        }

        public bool Contains (NodeDisplayBase node) {
            return Selected.Contains(node);
        }
    }
}
