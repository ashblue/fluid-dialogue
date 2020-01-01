using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Nodes;
using CleverCrow.Fluid.Utilities.UnityEvents;

namespace CleverCrow.Fluid.Dialogues {
    public class DialogueEvents : IDialogueEvents {
        public IUnityEvent Begin { get; } = new UnityEventPlus();
        public IUnityEvent End { get; } = new UnityEventPlus();
        public IUnityEvent<IActor, string> Speak { get; } = new UnityEventPlus<IActor, string>();
        public IUnityEvent<IActor, string, List<IChoice>> Choice { get; } = new UnityEventPlus<IActor, string, List<IChoice>>();
        public IUnityEvent<INode> NodeEnter { get; } = new UnityEventPlus<INode>();
    }
}
