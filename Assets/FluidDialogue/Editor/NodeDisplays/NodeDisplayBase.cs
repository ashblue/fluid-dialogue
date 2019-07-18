using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    public abstract class NodeDisplayBase {
        protected NodeDataBase Data { get; private set; }
        protected bool IsSelected { get; private set; }

        public void Setup (NodeDataBase data) {
            Data = data;
            OnSetup();
        }

        protected virtual void OnSetup () {
        }

        public virtual void Print () {
        }

        private void Select () {
            Selection.activeObject = Data;
            IsSelected = true;
        }

        private void Deselect () {
            IsSelected = false;
        }

        public void ProcessEvent (Event e) {
            switch (e.type) {
                case EventType.MouseDown when Data.rect.Contains(e.mousePosition):
                    Select();
                    GUI.changed = true;
                    break;
                case EventType.MouseDown:
                    if (IsSelected) {
                        Deselect();
                        GUI.changed = true;
                    }
                    break;
                case EventType.MouseDrag when IsSelected:
                    Data.rect.position += e.delta;
                    e.Use();
                    break;
            }
        }
    }
}
