using CleverCrow.Fluid.Dialogues.Builders;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeLinkTest {
        public class IsValidProperty {
            [Test]
            public void It_should_return_the_child_IsValid_status () {
                var child = A.Node.WithIsValid(true).Build();
                var link = new NodeLink(child);

                Assert.IsTrue(link.IsValid);
            }
        }

        public class NextMethod {
            [Test]
            public void It_should_return_its_child () {
                var child = A.Node.WithIsValid(true).Build();
                var link = new NodeLink(child);

                Assert.AreEqual(child, link.Next());
            }

            [Test]
            public void It_should_not_return_its_child_if_invalid () {
                var child = A.Node.WithIsValid(false).Build();
                var link = new NodeLink(child);

                Assert.IsNull(link.Next());
            }
        }
    }
}
