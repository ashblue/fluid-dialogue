using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Choices;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public interface INodeRuntime {
        List<IAction> ExitActions { get; }
        List<IAction> EnterActions { get; }
        bool IsValid { get; }

        INodeRuntime Next ();
        void Play (IDialogueEvents events);
        IChoiceRuntime GetChoice (int index);
    }
}
