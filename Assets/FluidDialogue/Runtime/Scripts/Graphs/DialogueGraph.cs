using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Graphs {
    public class DialogueGraph : ScriptableObject, IGraphClone {
        [SerializeField]
        private IDialogueNode _root;

        public IDialogueGraph Clone () {
            var graph = new DialogueGraphInternal(_root);
            return graph.Clone();
        }
    }

    public class DialogueGraphInternal : IDialogueGraph {
        public IDialogueNode Root { get; }

        public DialogueGraphInternal (IDialogueNode root) {
            Root = root;
        }

        public IDialogueGraph Clone () {
            return new DialogueGraphInternal(Root.Clone());
        }
    }
}
