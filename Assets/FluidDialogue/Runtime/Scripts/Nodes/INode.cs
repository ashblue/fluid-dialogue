using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Choices;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public interface INode {
        List<IAction> ExitActions { get; }
        List<IAction> EnterActions { get; }
        bool IsValid { get; }

        INode Next ();
        void Play (IDialogueEvents events);
        IChoice GetChoice (int index);
    }
}
