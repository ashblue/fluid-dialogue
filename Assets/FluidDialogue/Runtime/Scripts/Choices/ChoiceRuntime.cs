using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Choices {
    public class ChoiceRuntime : IChoice {
        private readonly List<INode> _children;

        public string UniqueId { get; }

        public ChoiceRuntime (string uniqueId, List<INode> children) {
            UniqueId = uniqueId;
            _children = children;
        }

        public INode GetValidChildNode () {
            return _children.Find(c => c.IsValid);
        }
    }
}
