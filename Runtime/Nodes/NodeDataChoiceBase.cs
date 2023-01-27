using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Choices;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public abstract class NodeDataChoiceBase : NodeDataBase {
        [HideInInspector]
        public List<ChoiceData> choices = new List<ChoiceData>();

        public override List<ChoiceData> Choices => choices;

        public override void ClearConnectionChildren () {
            base.ClearConnectionChildren();
            foreach (var choice in choices) {
                choice.ClearConnectionChildren();
            }
        }

        public override void SortConnectionsByPosition () {
            base.SortConnectionsByPosition();
            foreach (var choice in choices) {
                choice.SortConnectionsByPosition();
            }
        }

        public override NodeDataBase GetDataCopy () {
            var copy = base.GetDataCopy() as NodeDataChoiceBase;
            copy.choices = choices.Select(Instantiate).ToList();

            return copy;
        }
    }
}
