using CleverCrow.Fluid.Dialogues.Builders;
using CleverCrow.Fluid.Dialogues.Graphs;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeLinkTest {
        private INode _child;
        private NodeLink _link;
        private IGraph _graph;

        [SetUp]
        public void BeforeEach () {
            _child = A.Node.Build();
            var childData = A.NodeData.WithNode(_child).Build();
            _graph = A.Graph.WithNode(childData).Build();
            _link = new NodeLink(_graph, null, childData, null, null, null);
        }

        public class IsValidProperty : NodeLinkTest {
            [Test]
            public void It_should_return_the_child_IsValid_status () {
                _child.IsValid.Returns(true);

                Assert.IsTrue(_link.IsValid);
            }

            [Test]
            public void It_should_not_crash_if_there_is_no_child () {
                _link = new NodeLink(_graph, null, null, null, null, null);

                Assert.DoesNotThrow(() => {
                    var value = _link.IsValid;
                    Assert.IsFalse(value);
                });
            }
        }

        public class NextMethod : NodeLinkTest {
            [Test]
            public void It_should_return_its_child () {
                _child.IsValid.Returns(true);

                Assert.AreEqual(_child, _link.Next());
            }

            [Test]
            public void It_should_not_return_its_child_if_invalid () {
                _child.IsValid.Returns(false);

                Assert.IsNull(_link.Next());
            }
        }

        public class PlayMethod : NodeLinkTest {
            [Test]
            public void It_should_trigger_next_on_playback () {
                var playback = Substitute.For<IDialoguePlayback>();

                _link.Play(playback);

                playback.Received(1).Next();
            }
        }
    }
}
