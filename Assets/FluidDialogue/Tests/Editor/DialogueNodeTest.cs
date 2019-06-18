using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Builders;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeDialogueTest {
        public class NextMethod {
            [Test]
            public void It_should_return_a_child_with_IsValid_true () {
                var child = A.Node
                    .WithIsValid(true)
                    .Build();
                var children = new List<INodeRuntime> { child };
                var node = new NodeDialogue(children);

                var result = node.Next();

                Assert.AreEqual(child, result);
            }

            [Test]
            public void It_should_return_null_if_all_children_are_invalid () {
                var child = A.Node
                    .WithIsValid(false)
                    .Build();
                var children = new List<INodeRuntime> { child };
                var node = new NodeDialogue(children);

                var result = node.Next();

                Assert.IsNull(result);
            }
        }

        public class IsValid {
            public void It_should_return_true_if_all_conditions_are_true () {

            }

            public void It_should_return_false_if_all_conditions_are_false () {

            }
        }

        public class PlayMethod {
            // @TODO Trigger speak event
            // * Does not trigger choice event at the same time
            // @TODO Trigger choice event if choices
            // * Does not trigger speak event at same time
        }

        public class ExitActionsProperty {
            public void It_should_be_populated_by_the_constructor () {
            }
        }

        public class EnterActionsProperty {
            public void It_should_be_populated_by_the_constructor () {
            }
        }

        public class GetChoiceMethod {
            // @TODO It should return the choice by index (cached in Play)
        }
    }
}
