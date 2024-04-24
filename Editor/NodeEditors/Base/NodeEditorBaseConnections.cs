using System;
using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    public abstract partial class NodeEditorBase {
        private readonly List<Connection> _connections = new List<Connection>();
        private readonly List<Connection> _out = new List<Connection>();
        private readonly List<Connection> _in = new List<Connection>();

        protected virtual bool HasOutConnection => true;
        protected virtual bool HasInConnection => true;

        public IReadOnlyList<Connection> Out => _out;
        public IReadOnlyList<Connection> In => _in;

        public Connection GetConnection (Vector2 mousePosition) {
            return _connections.Find(c => c.IsClicked(mousePosition));
        }

        public void CreateConnection (ConnectionType type, IConnectionChildCollection childCollection, bool isFirst) {
            var connection = new Connection(type, Data, childCollection, Window, isFirst);

            _connections.Add(connection);
            switch (type) {
                case ConnectionType.In:
                    _in.Add(connection);
                    break;
                case ConnectionType.Out:
                    _out.Add(connection);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void RemoveConnection (Connection connection) {
            _connections.Remove(connection);
            switch (connection.Type) {
                case ConnectionType.In:
                    _in.Remove(connection);
                    break;
                case ConnectionType.Out:
                    _out.Remove(connection);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(connection.Type), connection.Type, null);
            }
        }

        private void PositionConnections () {
            if (Out.Count > 0) {
                var outPosition = Data.rect.position;
                outPosition.x += Data.rect.width - Connection.SIZE / 2;
                outPosition.y += Data.rect.height / 2 - Connection.SIZE / 2;
                Out[0].SetPosition(outPosition);
            }

            if (In.Count > 0) {
                var inPosition = Data.rect.position;
                inPosition.x -= Connection.SIZE / 2;
                inPosition.y += Data.rect.height / 2 - Connection.SIZE / 2;
                In[0].SetPosition(inPosition);
            }
        }

        public void CleanConnections () {
            foreach (var connection in In) {
                foreach (var parent in connection.Parents) {
                    parent.UndoRecordAllObjects();
                    parent.Links.RemoveLink(connection);
                }
            }
        }

        private void PrintConnections () {
            foreach (var connection in _connections) {
                connection.Print();
            }
        }
    }
}
