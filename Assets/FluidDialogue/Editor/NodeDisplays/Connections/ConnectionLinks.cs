using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    public partial interface IConnection {
        void AddConnection (IConnection connection);
        void RemoveConnection (IConnection connection);
    }

    public partial class Connection {
        private readonly List<IConnection> _connections = new List<IConnection>();

        public void AddConnection (IConnection target) {
            if (target == null
                || target.Type == _type
                || _connections.Contains(target)) return;

            if (_type == ConnectionType.In) {
                target.AddConnection(this);
                return;
            }

            BindConnection(target);

            if (_data is Object data) {
                Undo.RecordObject(data, "Add connection");
            }

            _data.Children.Add(target.Data as NodeDataBase);
        }

        public void RemoveConnection (IConnection connection) {
            _connections.Remove(connection);
            _data.Children.Remove(connection.Data as NodeDataBase);
        }

        public void RebuildConnections () {
            if (_type == ConnectionType.In) {
                return;
            }

            ClearAllConnections();
            foreach (var child in _data.Children) {
                var target = _window.DataToNode[child];
                BindConnection(target.In);
            }
        }

        private void BindConnection (IConnection target) {
            _connections.Add(target);
            target.AddParent(this);
        }

        private void ClearAllConnections () {
            _connections.ForEach(c => c?.RemoveParent(this));
            _connections.Clear();
        }
    }
}
