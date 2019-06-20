using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Choices {
    public class ChoiceData : ScriptableObject, IGetRuntime<IChoice> {
        public List<NodeDataBase> children;

        public IChoice GetRuntime () {
            return new ChoiceRuntime(children.Select(c => c.GetRuntime()).ToList());
        }
    }
}
