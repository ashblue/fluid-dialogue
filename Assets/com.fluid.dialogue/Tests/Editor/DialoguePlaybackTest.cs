using System.Collections.Generic;
using CleverCrow.Fluid.Dialogues.Actions;
using CleverCrow.Fluid.Dialogues.Builders;
using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Graphs;
using CleverCrow.Fluid.Dialogues.Nodes;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues {
    public class DialoguePlaybackTest {
        private DialoguePlayback _playback;
        private IGraph _graph;
        private IDialogueEvents _events;

        [SetUp]
        public void BeforeEach () {
            _graph = A.Graph
                .WithNextResult(A.Node.Build())
                .Build();

            _events = Substitute.For<IDialogueEvents>();
            _playback = new DialoguePlayback(_graph, null, _events);
        }

        public class PlayMethod {
            public class Defaults : DialoguePlaybackTest {
                [Test]
                public void It_should_trigger_a_Begin_event () {
                    _playback.Play();

                    _playback.Events.Begin.Received(1).Invoke();
                }

                [Test]
                public void It_should_trigger_a_speak_event_with_the_root_child_dialogue () {
                    var node = A.Node.Build();
                    node.Play(Arg.Do<IDialoguePlayback>(p => p.Events.Speak.Invoke(null, null)));

                    _graph = A.Graph
                        .WithNextResult(node)
                        .Build();
                    _playback = new DialoguePlayback(_graph, null, _events);

                    _playback.Play();

                    _playback.Events.Speak.ReceivedWithAnyArgs(1).Invoke(null, null);
                }

                [Test]
                public void It_should_trigger_play_on_the_root_child () {
                    var node = A.Node.Build();
                    _graph = A.Graph
                        .WithNextResult(node)
                        .Build();
                    _playback = new DialoguePlayback(_graph, null, _events);

                    _playback.Play();

                    node.Received(1).Play(_playback);
                }
            }

            public class RootNode : DialoguePlaybackTest {
                [Test]
                public void It_should_trigger_play_on_the_root_node () {
                    _playback.Play();

                    _graph.Root.Received(1).Play(_playback);
                }

                [Test]
                public void It_should_trigger_end_event_if_root_isValid_is_false () {
                    _graph.Root.IsValid.Returns(false);

                    _playback.Play();

                    _playback.Events.End.Received(1).Invoke();
                }
            }

            public class RootEnterActions : DialoguePlaybackTest {
                private IAction _action;

                [SetUp]
                public void BeforeEachMethod () {
                    _action = A.Action
                        .WithTickStatus(ActionStatus.Continue)
                        .Build();
                    _graph.Root.EnterActions.Returns(new List<IAction> {_action});
                }

                [Test]
                public void It_should_run_root_enter_actions () {
                    _playback.Play();

                    _action.Received(1).Tick();
                }

                [Test]
                public void It_should_call_begin_event_on_action_failure () {
                    _playback.Play();

                    _playback.Events.Begin.Received(1).Invoke();
                }

                [Test]
                public void If_root_enter_action_returns_false_do_not_call_speak () {
                    var node = A.Node.Build();
                    _graph = A.Graph
                        .WithNextResult(node)
                        .Build();
                    _graph.Root.EnterActions.Returns(new List<IAction> {_action});
                    _playback = new DialoguePlayback(_graph, null, _events);

                    _playback.Play();

                    node.DidNotReceive().Play(_playback);
                }

                [Test]
                public void Calling_Play_a_second_time_should_call_end_on_all_active_actions () {
                    _playback.Play();
                    _playback.Play();

                    _action.Received(1).End();
                }
            }
        }

        public class NextMethod {
            public class Defaults : DialoguePlaybackTest {
                [Test]
                public void It_should_trigger_speak_on_the_next_sibling_node () {
                    var nodeNested = A.Node.Build();
                    var node = A.Node
                        .WithNextResult(nodeNested)
                        .Build();
                    _graph = A.Graph
                        .WithNextResult(node)
                        .Build();
                    _playback = new DialoguePlayback(_graph, null, _events);

                    _playback.Play();
                    _playback.Next();

                    nodeNested.Received(1).Play(_playback);
                }

                [Test]
                public void If_no_next_result_trigger_end_event () {
                    var node = A.Node
                        .WithNextResult(null)
                        .Build();
                    _graph = A.Graph
                        .WithNextResult(node)
                        .Build();
                    _playback = new DialoguePlayback(_graph, null, _events);

                    _playback.Play();
                    _playback.Next();

                    _playback.Events.End.Received(1).Invoke();
                }

                [Test]
                public void It_should_trigger_end_event_only_once_when_an_action_is_present () {
                    var action = A.Action.Build();
                    var node = A.Node
                        .WithNextResult(null)
                        .WithEnterAction(action)
                        .Build();
                    _graph = A.Graph
                        .WithNextResult(node)
                        .Build();
                    _playback = new DialoguePlayback(_graph, null, _events);

                    _playback.Play();
                    _playback.Next();

                    _playback.Events.End.Received(1).Invoke();
                }
            }

            public class ChoiceHandling : DialoguePlaybackTest {
                [Test]
                public void It_should_emit_a_choice_event_if_next_node_has_choices () {
                    var choice = Substitute.For<IChoice>();
                    var nodeNested = A.Node
                        .WithChoice(choice)
                        .Build();
                    var node = A.Node
                        .WithNextResult(nodeNested)
                        .Build();
                    _graph = A.Graph
                        .WithNextResult(node)
                        .Build();
                    _playback = new DialoguePlayback(_graph, null, _events);

                    _playback.Play();
                    _playback.Next();

                    nodeNested.Received(1).Play(_playback);
                }
            }

            public class ExitActions : DialoguePlaybackTest {
                private IAction _exitAction;
                private INode _node;
                private INode _nodeNested;

                [SetUp]
                public void BeforeEachMethod () {
                    _exitAction = A.Action
                        .WithTickStatus(ActionStatus.Continue)
                        .Build();
                    _nodeNested = A.Node.Build();
                    _node = A.Node
                        .WithExitAction(_exitAction)
                        .WithNextResult(_nodeNested)
                        .Build();
                    _graph = A.Graph
                        .WithNextResult(_node)
                        .Build();

                    _playback = new DialoguePlayback(_graph, null, _events);
                    _playback.Play();
                    _playback.Next();
                }

                [Test]
                public void It_should_trigger_all_exit_actions_on_current () {
                    _exitAction.Received(1).Tick();
                }

                [Test]
                public void It_should_not_trigger_Speak_on_the_next_node_if_an_IAction_Update_returns_false () {
                    _nodeNested.DidNotReceive().Play(_playback);
                }

                [Test]
                public void It_should_trigger_Speak_on_the_next_node_if_an_IAction_Update_returns_false_then_true () {
                    _exitAction.Tick().Returns(ActionStatus.Success);
                    _playback.Tick();

                    _nodeNested.Received(1).Play(_playback);
                }

                [Test]
                public void It_should_do_nothing_if_actions_are_running () {
                    _playback.Next();

                    _exitAction.Received(1).Tick();
                }
            }

            public class EnterActions : DialoguePlaybackTest {
                private IAction _enterAction;
                private INode _node;

                [SetUp]
                public void BeforeEachMethod () {
                    _enterAction = A.Action
                        .WithTickStatus(ActionStatus.Continue)
                        .Build();
                    _node = A.Node
                        .WithEnterAction(_enterAction)
                        .Build();
                    _graph = A.Graph
                        .WithNextResult(_node)
                        .Build();

                    _playback = new DialoguePlayback(_graph, null, _events);
                }

                [Test]
                public void It_should_trigger_all_enter_actions_on_current () {
                    _playback.Play();
                    _playback.Next();

                    _enterAction.Received(1).Tick();
                }
            }
        }

        public class TickMethod : DialoguePlaybackTest {
            [Test]
            public void It_should_not_crash_when_updating_multiple_actions_at_the_same_time () {
                var action = A.Action
                    .WithTickStatus(ActionStatus.Continue)
                    .Build();
                var node = A.Node
                    .WithExitAction(action)
                    .WithExitAction(action)
                    .Build();
                var graph = A.Graph
                    .WithNextResult(node)
                    .Build();
                _playback = new DialoguePlayback(graph, null, _events);

                _playback.Play();
                _playback.Next();
                action.Tick().Returns(ActionStatus.Success);
                _playback.Tick();
            }
        }

        public class StopMethod : DialoguePlaybackTest {
            private IAction _action;

            [SetUp]
            public void BeforeEachMethod () {
                _action = A.Action
                    .WithTickStatus(ActionStatus.Continue)
                    .Build();
                _graph.Root.EnterActions.Returns(new List<IAction> {_action});
            }

            [Test]
            public void It_should_call_end_on_all_active_running_actions () {
                _playback.Play();
                _playback.Stop();

                _action.Received(1).End();
            }

            [Test]
            public void It_should_clear_the_pointer () {
                _playback.Play();
                _playback.Stop();

                Assert.IsNull(_playback.Pointer);
            }

            [Test]
            public void It_should_call_End_event () {
                _playback.Play();
                _playback.Stop();

                _playback.Events.End.Received(1).Invoke();
            }

            [Test]
            public void It_should_not_call_End_event_if_Play_was_not_called () {
                _playback.Stop();

                _playback.Events.End.DidNotReceive().Invoke();
            }
        }

        public class SelectChoiceMethod : DialoguePlaybackTest {
            [Test]
            public void It_should_trigger_the_current_pointer_choice_to_speak () {
                var choiceNode = A.Node.Build();
                var choice = Substitute.For<IChoice>();
                choice.GetValidChildNode().Returns(choiceNode);

                var nodeNested = A.Node
                    .WithChoice(choice)
                    .Build();
                var node = A.Node
                    .WithNextResult(nodeNested)
                    .Build();
                _graph = A.Graph
                    .WithNextResult(node)
                    .Build();
                _playback = new DialoguePlayback(_graph, null, _events);

                _playback.Play();
                _playback.Next();
                _playback.SelectChoice(0);

                choiceNode.Received(1).Play(_playback);
            }
        }
    }
}
