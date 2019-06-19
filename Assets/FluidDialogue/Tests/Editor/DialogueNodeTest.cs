using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Builders;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeDialogueTest {
        private const string DIALOGUE = "Lorem Ipsum";
        private IActor _actor;
        private List<INodeRuntime> _children;
        private List<IChoiceRuntime> _choiceList;

        [SetUp]
        public void BeforeEach () {
            _actor = Substitute.For<IActor>();
            _children = new List<INodeRuntime>();
            _choiceList = new List<IChoiceRuntime>();
        }

        public class NextMethod : NodeDialogueTest {
            [Test]
            public void It_should_return_a_child_with_IsValid_true () {
                var child = A.Node
                    .WithIsValid(true)
                    .Build();
                _children.Add(child);
                var node = new NodeDialogue(_actor, DIALOGUE, _children, _choiceList);

                var result = node.Next();

                Assert.AreEqual(child, result);
            }

            [Test]
            public void It_should_return_null_if_all_children_are_invalid () {
                var child = A.Node
                    .WithIsValid(false)
                    .Build();
                _children.Add(child);
                var node = new NodeDialogue(_actor, DIALOGUE, _children, _choiceList);

                var result = node.Next();

                Assert.IsNull(result);
            }
        }

        public class PlayMethod {
            public class SpeakEvents : NodeDialogueTest {
                [Test]
                public void It_should_trigger_a_speak_event () {
                    var child = A.Node
                        .WithIsValid(true)
                        .Build();
                    _children.Add(child);
                    var node = new NodeDialogue(_actor, DIALOGUE, _children, _choiceList);
                    var events = Substitute.For<IDialogueEvents>();

                    node.Play(events);

                    events.Speak.Received(1).Invoke(_actor, DIALOGUE);
                }
            }

            public class ChoiceEvents : NodeDialogueTest {
                [Test]
                public void It_should_trigger_a_choice_event_with_valid_choices () {
                    var choice = Substitute.For<IChoiceRuntime>();
                    choice.Node.IsValid.Returns(true);
                    _choiceList.Add(choice);
                    var node = new NodeDialogue(_actor, DIALOGUE, _children, _choiceList);
                    var events = Substitute.For<IDialogueEvents>();

                    node.Play(events);

                    events.Choice.ReceivedWithAnyArgs(1).Invoke(_actor, DIALOGUE, _choiceList);
                }

                [Test]
                public void It_should_not_trigger_a_choice_and_speak_event () {
                    var choice = Substitute.For<IChoiceRuntime>();
                    choice.Node.IsValid.Returns(true);
                    _choiceList.Add(choice);
                    var node = new NodeDialogue(_actor, DIALOGUE, _children, _choiceList);
                    var events = Substitute.For<IDialogueEvents>();

                    node.Play(events);

                    events.Speak.DidNotReceive().Invoke(_actor, DIALOGUE);
                }

                [Test]
                public void It_should_not_trigger_a_choice_event_with_invalid_choices () {
                    var choice = Substitute.For<IChoiceRuntime>();
                    choice.Node.IsValid.Returns(false);
                    _choiceList.Add(choice);
                    var node = new NodeDialogue(_actor, DIALOGUE, _children, _choiceList);
                    var events = Substitute.For<IDialogueEvents>();

                    node.Play(events);

                    events.Choice.DidNotReceive().Invoke(_actor, DIALOGUE, _choiceList);
                }

                [Test]
                public void It_should_not_trigger_a_choice_event_without_choices () {
                    var node = new NodeDialogue(_actor, DIALOGUE, _children, _choiceList);
                    var events = Substitute.For<IDialogueEvents>();

                    node.Play(events);

                    events.Choice.DidNotReceive().Invoke(_actor, DIALOGUE, _choiceList);
                }
            }
        }

        public class IsValid {
            public void It_should_return_true_if_all_conditions_are_true () {

            }

            public void It_should_return_false_if_all_conditions_are_false () {

            }
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
