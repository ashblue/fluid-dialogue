using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Builders;
using CleverCrow.Fluid.Dialogues.Choices;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeChoiceHubTest {
        private NodeChoiceHub _choiceHub;
        private List<IChoice> _choiceList;

        [SetUp]
        public void BeforeEach () {
            _choiceList = new List<IChoice>();
            _choiceHub = new NodeChoiceHub(_choiceList);
        }

        public class HubChoicesProperty : NodeChoiceHubTest {
            [Test]
            public void It_should_return_choices_with_a_valid_child_node () {
                var choice = Substitute.For<IChoice>();
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
