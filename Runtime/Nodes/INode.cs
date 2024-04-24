using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Conditions;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public interface INode : IUniqueId {
        List<IAction> EnterActions { get; }
        List<IAction> ExitActions { get; }
        bool IsValid { get; }
        List<IChoice> HubChoices { get; }
        IReadOnlyList<ICondition> Conditions { get; }

        /// <summary>
        /// Returns the first valid child node
        /// </summary>
        INode Next ();
        void Play (IDialoguePlayback playback);
        IChoice GetChoice (int index);
    }
}
