using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Editors.NodeDisplays;
using CleverCrow.Fluid.Dialogues.Nodes;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class ConnectionTest {
        public class AddConnectionMethod {
            [Test]
            public void It_should_add_a_parent_to_the_in_connection () {
                var cOut = new Connection(ConnectionType.Out,
                    Substitute.For<INodeData>(),
                    Substitute.For<IDialogueWindow>());
                cOut.Data.Children.Returns(new List<NodeDataBase>());

                var cIn = Substitute.For<IConnection>();

                cOut.AddConnection(cIn);

                cIn.Received(1).AddParent(cOut);
            }
        }
    }
}
