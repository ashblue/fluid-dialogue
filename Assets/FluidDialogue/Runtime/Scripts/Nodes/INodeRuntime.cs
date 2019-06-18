using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public interface INodeRuntime {
        List<IAction> ExitActions { get; }
        List<IAction> EnterActions { get; }
        bool IsValid { get; }

        INodeRuntime Next ();
        void Play (DialoguePlayback playback);
        IChoiceRuntime GetChoice (int index);
    }
}
