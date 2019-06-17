using System.Collections.Generic;

namespace CleverCrow.Fluid.Dialogues {
    public interface IDialogueEvents {
        IUnityEvent Begin { get; }
        IUnityEvent End { get; }
        IUnityEvent<IActor, string> Speak { get; }
        IUnityEvent<IActor, string, List<IChoice>> Choice { get; }
    }
}
