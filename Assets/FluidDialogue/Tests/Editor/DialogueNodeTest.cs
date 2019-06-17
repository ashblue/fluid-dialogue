using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Builders;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class DialogueNodeTest {
        public class GetValidChildMethod {
            [Test]
            public void It_should_return_the_first_child_with_valid_conditions () {
                var child = A.Node
                    .WithIsValid(true)
                    .Build();
                var children = new List<IDialogueNode> { child };

                var result = DialogueNodeInternal.GetValidChild(children);

                Assert.AreEqual(child, result);
            }

            [Test]
            public void It_should_return_null_if_all_children_are_invalid () {
                var child = A.Node
                    .WithIsValid(false)
                    .Build();
                var children = new List<IDialogueNode> { child };

                var result = DialogueNodeInternal.GetValidChild(children);

                Assert.IsNull(result);
            }
        }

        public class GetValidChoicesMethod {
            [Test]
            public void It_should_return_choices_with_IsValid_child_nodes () {
                var child = A.Node
                    .WithIsValid(true)
                    .Build();
                var choice = Substitute.For<IChoice>();
                choice.Node.Returns(child);
                var choiceList = new List<IChoice> {choice};

                var result = DialogueNodeInternal.GetValidChoices(choiceList);

                Assert.AreEqual(choiceList, result);
            }

            [Test]
            public void It_should_not_return_choices_with_IsValid_false_child_nodes () {
                var child = A.Node
                    .WithIsValid(false)
                    .Build();
                var choice = Substitute.For<IChoice>();
                choice.Node.Returns(child);
                var choiceList = new List<IChoice> {choice};

                var result = DialogueNodeInternal.GetValidChoices(choiceList);

                Assert.AreEqual(0, result.Count);
            }
        }
    }
}
