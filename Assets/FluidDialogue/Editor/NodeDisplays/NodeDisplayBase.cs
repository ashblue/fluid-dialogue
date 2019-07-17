using CleverCrow.Fluid.Dialogues.Nodes;
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
            IsSelected = true;
        }

        private void Deselect () {
            IsSelected = false;
        }

        public void ProcessEvent (Event e) {
            if (e.type != EventType.MouseDown) return;

            if (Data.rect.Contains(e.mousePosition)) {
                Select();
                GUI.changed = true;
            } else if (IsSelected) {
                Deselect();
                GUI.changed = true;
            }
        }
    }
}
