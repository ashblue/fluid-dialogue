using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using Object = UnityEngine.Object;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    public interface IConnectionLinks {
        IReadOnlyList<IConnection> Connections { get; }

        void AddLink (IConnection connection);
        void RemoveLink (IConnection connection);
        void RebuildLinks ();
        void ClearAllLinks ();
    }

    public class ConnectionLinks : IConnectionLinks {
        private readonly Connection _owner;
        private readonly List<IConnection> _connections = new List<IConnection>();

        public IReadOnlyList<IConnection> Connections => _connections;

        public ConnectionLinks (Connection owner) {
            _owner = owner;
        }

        public void AddLink (IConnection target) {
            if (target == null
                || target.Type == _owner.Type
                || _connections.Contains(target)) return;

            if (_owner.Type == ConnectionType.In) {
                target.Links.AddLink(_owner);
                return;
            }

            BindLink(target);

            if (_owner.Data is Object data) {
                Undo.RecordObject(data, "Add connection");
            }

            _owner.Data.Children.Add(target.Data as NodeDataBase);
        }

        public void RemoveLink (IConnection connection) {
            _connections.Remove(connection);
            _owner.Data.Children.Remove(connection.Data as NodeDataBase);
        }

        public void RebuildLinks () {
            if (_owner.Type == ConnectionType.In) {
                return;
            }

            ClearAllLinks();
            foreach (var child in _owner.Data.Children) {
                var target = _owner.Window.DataToNode[child];
                BindLink(target.In);
            }
        }

        private void BindLink (IConnection target) {
            _connections.Add(target);
            target.AddParent(_owner);
        }

        public void ClearAllLinks () {
            _connections.ForEach(c => c?.RemoveParent(_owner));
            _connections.Clear();
        }
    }
}
