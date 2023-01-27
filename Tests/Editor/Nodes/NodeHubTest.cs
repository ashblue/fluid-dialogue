using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Builders;
using CleverCrow.Fluid.Dialogues.Conditions;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeHubTest {
        private List<INodeData> _children;
        private List<ICondition> _conditions;

        [SetUp]
        public void BeforeEach () {
            _conditions = new List<ICondition>();
            _children = new List<INodeData>();
        }

        private NodeHub CreateNodeHub () {
            var graphBuilder = A.Graph;
            _children.ForEach(c => graphBuilder.WithNode(c));

            return new NodeHub(graphBuilder.Build(), null, _children, _conditions, null, null);
        }

        public class NextMethod : NodeHubTest {
            [Test]
            public void It_should_return_a_child_with_IsValid_true () {
                var child = A.Node
                    .WithIsValid(true)
                    .Build();
                var childData = A.NodeData.WithNode(child).Build();
                _children.Add(childData);
                var hub = CreateNodeHub();

                var result = hub.Next();

                Assert.AreEqual(child, result);
            }

            [Test]
            public void It_should_return_null_if_all_children_are_invalid () {
                var child = A.Node
                    .WithIsValid(false)
                    .Build();
                var childData = A.NodeData.WithNode(child).Build();
                _children.Add(childData);
                var hub = CreateNodeHub();

                var result = hub.Next();

                Assert.IsNull(result);
            }
        }

        public class PlayMethod : NodeHubTest {
            [Test]
            public void It_should_trigger_next_on_playback () {
                var playback = Substitute.For<IDialoguePlayback>();

                var hub = CreateNodeHub();
                hub.Play(playback);

                playback.Received(1).Next();
            }
        }

        public class IsValidProperty : NodeHubTest {
            [Test]
            public void It_should_return_true_if_all_conditions_are_true () {
                var condition = Substitute.For<ICondition>();
                condition.GetIsValid(Arg.Any<INode>()).Returns(true);
                _conditions.Add(condition);
                var hub = CreateNodeHub();

                Assert.IsTrue(hub.IsValid);
            }

            [Test]
            public void It_should_return_false_if_all_conditions_are_false () {
                var condition = Substitute.For<ICondition>();
                condition.GetIsValid(Arg.Any<INode>()).Returns(false);
                _conditions.Add(condition);
                var hub = CreateNodeHub();

                Assert.IsFalse(hub.IsValid);
            }
        }
    }
}
