using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public interface IDialogueNode {
        string Dialogue { get; }
        IActor Actor { get; }
        List<IAction> ExitActions { get; }
        List<IAction> EnterActions { get; }
        bool IsValid { get; }

        IDialogueNode Next ();
        List<IChoice> GetChoices ();
    }
}
