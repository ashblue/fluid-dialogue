using System.Collections.Generic;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    public partial interface IConnection {
        void AddParent (IConnection parent);
        void RemoveParent (IConnection parent);
    }

    public partial class Connection {
        private readonly List<IConnection> _parents = new List<IConnection>();

        public IReadOnlyList<IConnection> Parents => _parents;

        public void AddParent (IConnection parent) {
            _parents.Add(parent);
        }

        public void RemoveParent (IConnection parent) {
            _parents.Remove(parent);
        }
    }
}
