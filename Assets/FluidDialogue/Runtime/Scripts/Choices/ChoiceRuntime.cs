using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEngine.UI;

namespace CleverCrow.Fluid.Dialogues.Choices {
    public class ChoiceRuntime : IChoice {
        private readonly List<INode> _children;

        public string UniqueId { get; }
        public string Text { get; }

        public ChoiceRuntime (string text, string uniqueId, List<INode> children) {
            Text = text;
            UniqueId = uniqueId;
            _children = children;
        }

        public INode GetValidChildNode () {
            return _children.Find(c => c.IsValid);
        }
    }
}
