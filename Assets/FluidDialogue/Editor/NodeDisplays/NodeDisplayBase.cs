using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    public abstract class NodeDisplayBase {
        protected NodeDataBase _data;

        public void Setup (NodeDataBase data) {
            _data = data;
            OnSetup();
        }

        protected virtual void OnSetup () {
        }

        public virtual void Print () {
        }
    }
}
