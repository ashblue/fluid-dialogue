using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Choices {
    public interface IChoiceRuntime {
        INodeRuntime GetValidChildNode ();
    }

    public class ChoiceRuntime : IChoiceRuntime {
        private readonly List<INodeRuntime> _children;

        public ChoiceRuntime (List<INodeRuntime> children) {
            _children = children;
        }

        public INodeRuntime GetValidChildNode () {
            return _children.Find(c => c.IsValid);
        }
    }
}
