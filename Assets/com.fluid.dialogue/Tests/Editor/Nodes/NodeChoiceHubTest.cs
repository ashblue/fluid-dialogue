using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Builders;
using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Conditions;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeChoiceHubTest {
        private NodeChoiceHub _choiceHub;
        private List<IChoice> _choiceList;
        private List<ICondition> _conditions;

        [SetUp]
        public void BeforeEach () {
            _choiceList = new List<IChoice>();
            _conditions = new List<ICondition>();
            _choiceHub = new NodeChoiceHub(null, _choiceList, _conditions);
        }

        public class IsValidProperty : NodeChoiceHubTest {
            [Test]
            public void It_should_return_true () {
                Assert.IsTrue(_choiceHub.IsValid);
            }

            [Test]
            public void It_should_return_true_if_all_conditions_are_true () {
                var condition = Substitute.For<ICondition>();
                condition.GetIsValid(Arg.Any<INode>()).Returns(true);
                _conditions.Add(condition);

                Assert.IsTrue(_choiceHub.IsValid);
            }

            [Test]
            public void It_should_return_false_if_any_conditions_are_false () {
                var condition = Substitute.For<ICondition>();
                condition.GetIsValid(Arg.Any<INode>()).Returns(false);
                _conditions.Add(condition);

                Assert.IsFalse(_choiceHub.IsValid);
            }
        }

        public class HubChoicesProperty : NodeChoiceHubTest {
            [Test]
            public void It_should_return_choices_with_a_valid_child_node () {
                var choice = Substitute.For<IChoice>();
                choice.IsValid.Returns(true);
                choice.GetValidChildNode()
                    .Returns(x => A.Node.Build());
                _choiceList.Add(choice);

                Assert.IsTrue(_choiceHub.HubChoices.Contains(choice));
            }

            [Test]
            public void It_should_not_return_choices_with_an_invalid_child_node () {
                var choice = Substitute.For<IChoice>();
                choice.GetValidChildNode()
                    .Returns(x => null);
                _choiceList.Add(choice);

                Assert.IsFalse(_choiceHub.HubChoices.Contains(choice));
            }
        }
    }
}
