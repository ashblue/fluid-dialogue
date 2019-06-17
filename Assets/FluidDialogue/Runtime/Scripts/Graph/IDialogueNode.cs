using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;

namespace CleverCrow.Fluid.Dialogues.Graphs {
    public interface IDialogueNode {
        string Dialogue { get; }
        IActor Actor { get; }
        List<IAction> ExitActions { get; }
        List<IAction> EnterActions { get; }

        IDialogueNode Next ();
        List<IChoice> GetChoices ();
    }
}
