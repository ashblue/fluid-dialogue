using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Choices {
    public class ChoiceRuntime : IChoice {
        private readonly IGraph _runtime;
        private readonly List<INodeData> _children;
        private List<INode> _childrenRuntimeCache;

        public string UniqueId { get; }
        public string Text { get; }
        public bool IsValid => Children.Count == 0 || Children.Find(c => c.IsValid) != null;

        private List<INode> Children =>
            _childrenRuntimeCache ??
            (_childrenRuntimeCache = _children.Select(_runtime.GetCopy).ToList());

        public ChoiceRuntime (IGraph runtime, string text, string uniqueId, List<INodeData> children) {
            _runtime = runtime;
            Text = text;
            UniqueId = uniqueId;
            _children = children;
        }

        public INode GetValidChildNode () {
            return Children.Find(c => c.IsValid);
        }
    }
}
