using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Choices;

namespace CleverCrow.Fluid.Dialogues {
    public interface IDialogueEvents {
        IUnityEvent Begin { get; }
        IUnityEvent End { get; }
        IUnityEvent<IActor, string> Speak { get; }
        IUnityEvent<IActor, string, List<IChoice>> Choice { get; }
    }
}
