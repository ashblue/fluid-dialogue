using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Conditions;
using CleverCrow.Fluid.Dialogues.Graphs;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeDialogue : NodeBase {
        private readonly IActor _actor;
        private readonly string _dialogue;
        private readonly List<IChoice> _choices;

        private List<IChoice> _emittedChoices;
        private readonly AudioClip _audioClip;

        public NodeDialogue (
            IGraph graph,
            string uniqueId,
            IActor actor,
            string dialogue,
            AudioClip audioClip,
            List<INodeData> children,
            List<IChoice> choices,
            List<ICondition> conditions,
            List<IAction> enterActions,
            List<IAction> exitActions) :
            base(graph, uniqueId, children, conditions, enterActions, exitActions) {
            _actor = actor;
            _dialogue = dialogue;
            _choices = choices;
            _audioClip = audioClip;
        }

        private List<IChoice> GetValidChoices (IDialoguePlayback playback) {
            var child = Next();
            if (_choices.Count == 0 && child?.HubChoices != null && child.HubChoices.Count > 0) {
                playback.Events.NodeEnter.Invoke(child);
                return child.HubChoices;
            }

            return _choices.Where(c => c.IsValid).ToList();
        }

        protected override void OnPlay (IDialoguePlayback playback) {
            _emittedChoices = GetValidChoices(playback);
            if (_emittedChoices.Count > 0) {
                playback.Events.Choice.Invoke(_actor, _dialogue, _emittedChoices);
                return;
            }

            playback.Events.Speak.Invoke(_actor, _dialogue);
            playback.Events.SpeakWithAudio.Invoke(_actor, _dialogue, _audioClip);
        }

        public override IChoice GetChoice (int index) {
            if (index >= _emittedChoices.Count) return null;

            return _emittedChoices[index];
        }
    }
}
