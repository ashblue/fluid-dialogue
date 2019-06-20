using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Choices {
    public class ChoiceRuntime : IChoice {
        private readonly List<INode> _children;

        public ChoiceRuntime (List<INode> children) {
            _children = children;
        }

        public INode GetValidChildNode () {
            return _children.Find(c => c.IsValid);
        }
    }
}
