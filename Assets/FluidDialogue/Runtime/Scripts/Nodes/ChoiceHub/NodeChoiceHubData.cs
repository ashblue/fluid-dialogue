using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Choices;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeChoiceHubData : NodeDataBase {
        public List<ChoiceData> choices;

        public override INode GetRuntime () {
            var runtimeChoices = choices.Select(c => c.GetRuntime()).ToList();
            return new NodeChoiceHub(runtimeChoices);
        }
    }
}
