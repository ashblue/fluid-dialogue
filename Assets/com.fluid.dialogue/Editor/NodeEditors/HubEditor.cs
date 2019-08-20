using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.NodeDisplays {
    [NodeType(typeof(NodeHubData))]
    public class HubEditor : NodeEditorBase {
        protected override Color NodeColor { get; } = new Color(0.7f, 0.7f, 0.7f);
    }
}
