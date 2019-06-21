using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Builders;
using CleverCrow.Fluid.Dialogues.Conditions;
using CleverCrow.Fluid.Dialogues.Nodes.Hub;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeHubTest {
        private NodeHub _hub;
        private List<INode> _children;
        private List<ICondition> _conditions;

        [SetUp]
        public void BeforeEach () {
            _conditions = new List<ICondition>();
            _children = new List<INode>();
            _hub = new NodeHub(_children, _conditions, null);
        }

        public class NextMethod : NodeHubTest {
            [Test]
            public void It_should_return_a_child_with_IsValid_true () {
                var child = A.Node
                    .WithIsValid(true)
                    .Build();
                _children.Add(child);

                var result = _hub.Next();

                Assert.AreEqual(child, result);
            }

            [Test]
            public void It_should_return_null_if_all_children_are_invalid () {
                var child = A.Node
                    .WithIsValid(false)
                    .Build();
                _children.Add(child);

                var result = _hub.Next();

                Assert.IsNull(result);
            }
        }

        public class PlayMethod : NodeHubTest {
            [Test]
            public void It_should_trigger_next_on_playback () {
                var playback = Substitute.For<IDialoguePlayback>();

                _hub.Play(playback);

                playback.Received(1).Next();
            }
        }

        public class IsValidProperty : NodeHubTest {
            [Test]
            public void It_should_return_true_if_all_conditions_are_true () {
                var condition = Substitute.For<ICondition>();
                condition.GetIsValid().Returns(true);
                _conditions.Add(condition);

                Assert.IsTrue(_hub.IsValid);
            }

            [Test]
            public void It_should_return_false_if_all_conditions_are_false () {
                var condition = Substitute.For<ICondition>();
                condition.GetIsValid().Returns(false);
                _conditions.Add(condition);

                Assert.IsFalse(_hub.IsValid);
            }
        }
    }
}
