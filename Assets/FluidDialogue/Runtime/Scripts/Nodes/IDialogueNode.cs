using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public interface IDialogueNode {
        List<IAction> ExitActions { get; }
        List<IAction> EnterActions { get; }
        bool IsValid { get; }

        IDialogueNode Next ();
        void Play (DialoguePlayback playback);
        IChoice GetChoice (int index);
        IDialogueNode Clone ();
    }
}
