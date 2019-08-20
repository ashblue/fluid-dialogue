using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEditor;
using Object = UnityEngine.Object;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    public interface IConnectionLinks {
        IReadOnlyList<IConnection> List { get; }

        void AddLink (IConnection connection);
        void RemoveLink (IConnection connection);
        void RebuildLinks ();
        void ClearAllLinks ();
    }

    public class ConnectionLinks : IConnectionLinks {
        private readonly Connection _owner;
        private readonly List<IConnection> _list = new List<IConnection>();
        private readonly IConnectionChildCollection _childCollection;

        public IReadOnlyList<IConnection> List => _list;

        public ConnectionLinks (Connection owner, IConnectionChildCollection childCollection) {
            _owner = owner;
            _childCollection = childCollection;
        }

        public void AddLink (IConnection target) {
            if (target == null
                || target.Type == _owner.Type
                || !_owner.IsValidLinkTarget(target)
                || !target.IsValidLinkTarget(_owner)
                || _list.Contains(target)) return;

            if (_owner.Type == ConnectionType.In) {
                target.Links.AddLink(_owner);
                return;
            }

            BindLink(target);

            if (_childCollection is Object data) {
                Undo.RecordObject(data, "Add connection");
            }

            _childCollection.AddConnectionChild(target.Data);
            _childCollection.SortConnectionsByPosition();
        }

        public void RemoveLink (IConnection connection) {
            _list.Remove(connection);
            _childCollection.RemoveConnectionChild(connection.Data);
        }

        public void RebuildLinks () {
            if (_owner.Type == ConnectionType.In) {
                return;
            }

            ClearAllLinks();
            foreach (var child in _childCollection.Children) {
                var target = _owner.Window.DataToNode[child];
                BindLink(target.In[0]);
            }
        }

        public void ClearAllLinks () {
            _list.ForEach(c => c?.RemoveParent(_owner));
            _list.Clear();
        }

        private void BindLink (IConnection target) {
            _list.Add(target);
            target.AddParent(_owner);
        }
    }
}
