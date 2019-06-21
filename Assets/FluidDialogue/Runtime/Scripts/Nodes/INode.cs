using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Choices;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public interface INode {
        List<IAction> EnterActions { get; }
        List<IAction> ExitActions { get; }
        bool IsValid { get; }

        INode Next ();
        void Play (IDialoguePlayback playback);
        IChoice GetChoice (int index);
    }
}
