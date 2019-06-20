using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Builders;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeRootTest {
        public class ExitActionsProperty {
            [Test]
            public void It_should_populate_ExitActions () {
                var action = Substitute.For<IAction>();
                var exitActions = new List<IAction> {action};

                var root = new NodeRoot(null, exitActions);

                Assert.AreEqual(root.ExitActions, exitActions);
            }
        }

        public class IsValidProperty {
            [Test]
            public void It_should_return_true () {
                var root = new NodeRoot(null, null);

                Assert.IsTrue(root.IsValid);
            }
        }

        public class NextMethod {
            [Test]
            public void It_should_return_a_valid_child () {
                var child = A.Node.Build();
                var children = new List<INode> {child};
                var root = new NodeRoot(children, null);

                Assert.AreEqual(child, root.Next());
            }

            [Test]
            public void It_should_not_return_an_invalid_child () {
                var child = A.Node.WithIsValid(false).Build();
                var children = new List<INode> {child};
                var root = new NodeRoot(children, null);

                Assert.AreEqual(null, root.Next());
            }
        }
    }
}
