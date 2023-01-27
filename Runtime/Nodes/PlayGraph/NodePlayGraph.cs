using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Conditions;
using CleverCrow.Fluid.Dialogues.Graphs;

namespace CleverCrow.Fluid.Dialogues.Nodes.PlayGraph {
    public class NodePlayGraph : NodeBase {
        private readonly IGraphData _graph;

        public NodePlayGraph (
            IGraph runtime,
            string uniqueId,
            IGraphData graph,
            List<INodeData> children,
            List<ICondition> conditions,
            List<IAction> enterActions,
            List<IAction> exitActions)
            : base(runtime, uniqueId, children, conditions, enterActions, exitActions) {
            _graph = graph;
        }

        protected override void OnPlay (IDialoguePlayback playback) {
            playback.ParentCtrl.PlayChild(_graph);
        }
    }
}
