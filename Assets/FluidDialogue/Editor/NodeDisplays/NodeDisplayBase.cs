using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    public abstract class NodeDisplayBase {
        public NodeDataBase Data { get; private set; }
        public bool IsSelected { get; private set; }

        public void Setup (NodeDataBase data) {
            Data = data;
            OnSetup();
        }

        protected virtual void OnSetup () {
        }

        public virtual void Print () {
        }

        public void Select () {
            IsSelected = true;
        }

        public void Deselect () {
            IsSelected = false;
        }
    }
}
