using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    public abstract partial class NodeEditorBase {
        private readonly List<Connection> _connections = new List<Connection>();

        protected virtual bool HasOutConnection => true;
        protected virtual bool HasInConnection => true;

        public List<Connection> Out { get; } = new List<Connection>();
        public Connection In { get; private set; }

        public Connection GetConnection (Vector2 mousePosition) {
            return _connections.Find(c => c.IsClicked(mousePosition));
        }

        protected Connection CreateConnection (ConnectionType type) {
            var connection = new Connection(type, Data, Window);
            _connections.Add(connection);

            return connection;
        }

        private void PositionConnections () {
            if (Out.Count > 0) {
                var outPosition = Data.rect.position;
                outPosition.x += Data.rect.width - Connection.SIZE / 2;
                outPosition.y += Data.rect.height / 2 - Connection.SIZE / 2;
                Out[0].SetPosition(outPosition);
            }

            if (In != null) {
                var inPosition = Data.rect.position;
                inPosition.x -= Connection.SIZE / 2;
                inPosition.y += Data.rect.height / 2 - Connection.SIZE / 2;
                In.SetPosition(inPosition);
            }
        }

        public void CleanConnections () {
            foreach (var parent in In.Parents) {
                Undo.RecordObject((Object)parent.Data, "Removed connection");
                parent.Links.RemoveLink(In);
            }
        }

        private void PrintConnections () {
            foreach (var connection in _connections) {
                connection.Print();
            }
        }
    }
}
