using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public abstract class DialogueNodeBase : ScriptableObject, IDialogueNode {
        public abstract string Dialogue { get; }
        public abstract IActor Actor { get; }
        public abstract List<IAction> ExitActions { get; }
        public abstract List<IAction> EnterActions { get; }
        public abstract bool IsValid { get; }

        public abstract IDialogueNode Next ();

        public abstract List<IChoice> GetChoices ();
    }
}
