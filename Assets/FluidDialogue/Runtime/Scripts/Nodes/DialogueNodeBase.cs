using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public abstract class DialogueNodeBase : ScriptableObject, IDialogueNode {
        public abstract List<IAction> ExitActions { get; }
        public abstract List<IAction> EnterActions { get; }
        public abstract bool IsValid { get; }

        public abstract IDialogueNode Next ();
        public abstract void Play (DialoguePlayback playback);
        public abstract IChoice GetChoice (int index);
        public IDialogueNode Clone () {
            throw new System.NotImplementedException();
        }
    }
}
