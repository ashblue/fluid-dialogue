using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Nodes;
using CleverCrow.Fluid.Utilities.UnityEvents;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues {
    public interface IDialogueEvents {
        IUnityEvent Begin { get; }
        IUnityEvent End { get; }

        /// <summary>
        /// Old Speak event, use SpeakWithAudio to get audio sound files
        /// </summary>
        IUnityEvent<IActor, string> Speak { get; }
        IUnityEvent<IActor, string, AudioClip> SpeakWithAudio { get; }

        IUnityEvent<IActor, string, List<IChoice>> Choice { get; }
        IUnityEvent<INode> NodeEnter { get; }
    }
}
