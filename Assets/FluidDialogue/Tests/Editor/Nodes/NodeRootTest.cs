using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Builders;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeRootTest {
        public class NextMethod {
            [Test]
            public void It_should_return_a_valid_child () {
                var child = A.Node.Build();
                var children = new List<INode> {child};
                var root = new NodeRoot(null, children, null, null, null);

                Assert.AreEqual(child, root.Next());
            }

            [Test]
            public void It_should_not_return_an_invalid_child () {
                var child = A.Node.WithIsValid(false).Build();
                var children = new List<INode> {child};
                var root = new NodeRoot(null, children, null, null, null);

                Assert.AreEqual(null, root.Next());
            }
        }
    }
}
