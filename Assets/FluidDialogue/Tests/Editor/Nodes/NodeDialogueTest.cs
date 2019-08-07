using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Builders;
using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Conditions;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Nodes {
    public class NodeDialogueTest {
        private const string DIALOGUE = "Lorem Ipsum";
        private IActor _actor;
        private List<INode> _children;
        private List<IChoice> _choiceList;
        private List<ICondition> _conditions;
        private List<IAction> _enterActions;
        private List<IAction> _exitActions;

        [SetUp]
        public void BeforeEach () {
            _actor = Substitute.For<IActor>();
            _children = new List<INode>();
            _choiceList = new List<IChoice>();
            _conditions = new List<ICondition>();
            _enterActions = new List<IAction>();
            _exitActions = new List<IAction>();
        }

        private NodeDialogue CreateNodeDialogue () {
            return new NodeDialogue(
                null,
                _actor,
                DIALOGUE,
                _children,
                _choiceList,
                _conditions,
                _enterActions,
                _exitActions);
        }

        public class NextMethod : NodeDialogueTest {
            [Test]
            public void It_should_return_a_child_with_IsValid_true () {
                var child = A.Node
                    .WithIsValid(true)
                    .Build();
                _children.Add(child);
                var node = CreateNodeDialogue();

                var result = node.Next();

                Assert.AreEqual(child, result);
            }

            [Test]
            public void It_should_return_null_if_all_children_are_invalid () {
                var child = A.Node
                    .WithIsValid(false)
                    .Build();
                _children.Add(child);
                var node = CreateNodeDialogue();

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
                    var node = CreateNodeDialogue();
                    var playback = Substitute.For<IDialoguePlayback>();

                    node.Play(playback);

                    playback.Events.Speak.Received(1).Invoke(_actor, DIALOGUE);
                }
            }

            public class ChoiceEvents {
                public class InternalChoices : NodeDialogueTest {
                    [Test]
                    public void It_should_trigger_a_choice_event_with_valid_choices () {
                        var choice = Substitute.For<IChoice>();
                        _choiceList.Add(choice);
                        var node = CreateNodeDialogue();
                        var playback = Substitute.For<IDialoguePlayback>();

                        node.Play(playback);

                        playback.Events.Choice.ReceivedWithAnyArgs(1).Invoke(_actor, DIALOGUE, _choiceList);
                    }

                    [Test]
                    public void It_should_not_trigger_a_choice_and_speak_event () {
                        var choice = Substitute.For<IChoice>();
                        _choiceList.Add(choice);
                        var node = CreateNodeDialogue();
                        var playback = Substitute.For<IDialoguePlayback>();

                        node.Play(playback);

                        playback.Events.Speak.DidNotReceive().Invoke(_actor, DIALOGUE);
                    }

                    [Test]
                    public void It_should_not_trigger_a_choice_event_with_invalid_choices () {
                        var choice = Substitute.For<IChoice>();
                        choice.GetValidChildNode().Returns(x => null);
                        _choiceList.Add(choice);
                        var node = CreateNodeDialogue();
                        var playback = Substitute.For<IDialoguePlayback>();

                        node.Play(playback);

                        playback.Events.Choice.DidNotReceive().Invoke(_actor, DIALOGUE, _choiceList);
                    }

                    [Test]
                    public void It_should_not_trigger_a_choice_event_without_choices () {
                        var node = CreateNodeDialogue();
                        var playback = Substitute.For<IDialoguePlayback>();

                        node.Play(playback);

                        playback.Events.Choice.DidNotReceive().Invoke(_actor, DIALOGUE, _choiceList);
                    }
                }

                public class ChoiceHubs : NodeDialogueTest {
                    [Test]
                    public void It_should_use_choices_from_next_child_if_its_a_choice_hub () {
                        var playback = Substitute.For<IDialoguePlayback>();
                        var choice = Substitute.For<IChoice>();
                        var choiceHub = A.Node
                            .WithHubChoice(choice)
                            .Build();
                        _children.Add(choiceHub);

                        var node = CreateNodeDialogue();
                        node.Play(playback);

                        playback.Events.Choice.Received(1).Invoke(_actor, DIALOGUE, choiceHub.HubChoices);
                    }

                    [Test]
                    public void It_should_not_use_a_choice_hub_if_choices_are_present () {
                        var playback = Substitute.For<IDialoguePlayback>();
                        var choice = Substitute.For<IChoice>();
                        var choiceHub = A.Node
                            .WithHubChoice(choice)
                            .Build();
                        _children.Add(choiceHub);

                        _choiceList.Add(Substitute.For<IChoice>());
                        var node = CreateNodeDialogue();
                        node.Play(playback);

                        playback.Events.Choice.DidNotReceive().Invoke(_actor, DIALOGUE, choiceHub.HubChoices);
                    }

                    [Test]
                    public void It_should_not_use_a_choice_hub_if_its_invalid () {
                        var playback = Substitute.For<IDialoguePlayback>();
                        var choice = Substitute.For<IChoice>();
                        var choiceHub = A.Node
                            .WithHubChoice(choice)
                            .WithIsValid(false)
                            .Build();
                        _children.Add(choiceHub);

                        var node = CreateNodeDialogue();
                        node.Play(playback);

                        playback.Events.Choice.DidNotReceive().Invoke(_actor, DIALOGUE, choiceHub.HubChoices);
                    }

                    [Test]
                    public void It_should_not_error_if_choices_are_null () {
                        var playback = Substitute.For<IDialoguePlayback>();
                        var choiceHub = A.Node.Build();
                        List<IChoice> choices = null;
                        choiceHub.HubChoices.Returns(choices);
                        _children.Add(choiceHub);

                        var node = CreateNodeDialogue();
                        node.Play(playback);

                        Assert.DoesNotThrow(() => node.Play(playback));
                    }
                }
            }
        }

        public class IsValid : NodeDialogueTest {
            [Test]
            public void It_should_return_true_if_all_conditions_are_true () {
                var condition = Substitute.For<ICondition>();
                condition.GetIsValid().Returns(true);
                _conditions.Add(condition);
                var node = CreateNodeDialogue();

                Assert.IsTrue(node.IsValid);
            }

            [Test]
            public void It_should_return_false_if_any_conditions_are_false () {
                var condition = Substitute.For<ICondition>();
                condition.GetIsValid().Returns(false);
                _conditions.Add(condition);
                var node = CreateNodeDialogue();

                Assert.IsFalse(node.IsValid);
            }
        }

        public class EnterActionsProperty : NodeDialogueTest {
            [Test]
            public void It_should_be_populated_by_the_constructor () {
                var action = Substitute.For<IAction>();
                _enterActions.Add(action);

                var node = CreateNodeDialogue();

                Assert.IsTrue(node.EnterActions.Contains(action));
            }
        }

        public class ExitActionsProperty : NodeDialogueTest {
            [Test]
            public void It_should_be_populated_by_the_constructor () {
                var action = Substitute.For<IAction>();
                _exitActions.Add(action);

                var node = CreateNodeDialogue();

                Assert.IsTrue(node.ExitActions.Contains(action));
            }
        }

        public class GetChoiceMethod : NodeDialogueTest {
            [Test]
            public void It_should_return_valid_choices_emitted_by_Play () {
                var playback = Substitute.For<IDialoguePlayback>();
                var choice = Substitute.For<IChoice>();
                _choiceList.Add(choice);

                var node = CreateNodeDialogue();
                node.Play(playback);
                var result = node.GetChoice(0);

                Assert.AreEqual(choice, result);
            }

            [Test]
            public void It_should_not_return_invalid_choices_emitted_by_Play () {
                var playback = Substitute.For<IDialoguePlayback>();
                var choice = Substitute.For<IChoice>();
                choice.GetValidChildNode().Returns(x => null);
                _choiceList.Add(choice);

                var node = CreateNodeDialogue();
                node.Play(playback);
                var result = node.GetChoice(0);

                Assert.IsNull(result);
            }
        }
    }
}
