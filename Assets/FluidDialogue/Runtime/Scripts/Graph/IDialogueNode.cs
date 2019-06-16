using System.Collections.Generic;

namespace CleverCrow.Fluid.Dialogues.Graphs {
    public interface IDialogueNode {
        string Dialogue { get; }
        IActor Actor { get; }
        List<IAction> ExitActions { get; }
        List<IAction> EnterActions { get; }

        IDialogueNode Next ();
    }
}
