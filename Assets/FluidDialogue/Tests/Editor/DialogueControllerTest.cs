using System;
using CleverCrow.Fluid.Dialogues.Builders;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues {
    public class DialogueControllerTest {
        private DialogueController _ctrl;
        private IDialoguePlayback _playback;

        [SetUp]
        public void BeforeEach () {
            _ctrl = new DialogueController();
            _playback = Substitute.For<IDialoguePlayback>();
        }

        public class PlayMethod : DialogueControllerTest {
            [Test]
            public void It_should_run_play_on_the_graph () {
                _ctrl.Play(_playback);

                _playback.Received(1).Play();
            }

            [Test]
            public void It_should_call_Stop_on_already_running_dialogue () {
                _ctrl.Play(_playback);
                _ctrl.Play(_playback);

                _playback.Received(1).Stop();
            }

            [Test]
            public void It_should_bind_the_begin_event () {
                var beginResult = false;
                _ctrl.Events.Begin.AddListener(() => beginResult = true);
                var playback = new DialoguePlayback(A.Graph.Build(), null, new DialogueEvents());

                _ctrl.Play(playback);

                Assert.IsTrue(beginResult);
            }

            [Test]
            public void It_should_bind_the_end_event () {
                var endResult = false;
                _ctrl.Events.End.AddListener(() => endResult = true);
                var playbackEmpty = new DialoguePlayback(A.Graph.Build(), null, new DialogueEvents());

                _ctrl.Play(playbackEmpty);
                _ctrl.Next();

                Assert.IsTrue(endResult);
            }

            [Test]
            public void It_should_bind_dialogue_speak_events () {
                var speakResult = false;
                _ctrl.Events.Speak.AddListener((x, y) => speakResult = true);
                var playback = new DialoguePlayback(A.Graph.Build(), null, new DialogueEvents());

                _ctrl.Play(playback);
                playback.Events.Speak.Invoke(null, null);

                Assert.IsTrue(speakResult);
            }

            [Test]
            public void It_should_bind_dialogue_choice_events () {
                var choiceResult = false;
                _ctrl.Events.Choice.AddListener((x, y, z) => choiceResult = true);
                var playback = new DialoguePlayback(A.Graph.Build(), null, new DialogueEvents());

                _ctrl.Play(playback);
                playback.Events.Choice.Invoke(null, null, null);

                Assert.IsTrue(choiceResult);
            }
        }

        public class PlayChildMethod {
            public class ErrorHandling : DialogueControllerTest {
                [Test]
                public void It_should_throw_an_error_if_nothing_is_playing () {
                    Assert.Throws<InvalidOperationException>(
                        () => _ctrl.PlayChild(_playback), "Cannot trigger child dialogue, nothing is playing");
                }
            }

            public class SuccessfulRuns : DialogueControllerTest {
                private IDialoguePlayback _parentPlayback;

                [SetUp]
                public void BeforeEachMethod () {
                    _parentPlayback = Substitute.For<IDialoguePlayback>();
                    _ctrl.Play(_parentPlayback);
                    _ctrl.Next();
                }

                [Test]
                public void It_should_call_Play_on_the_dialogue () {
                    _ctrl.PlayChild(_playback);

                    _playback.Received(1).Play();
                }

                [Test]
                public void It_should_bind_the_speak_event () {
                    var speakResult = false;
                    _ctrl.Events.Speak.AddListener((x, y) => speakResult = true);
                    var playback = new DialoguePlayback(A.Graph.Build(), null, new DialogueEvents());

                    _ctrl.PlayChild(playback);
                    playback.Events.Speak.Invoke(null, null);

                    Assert.IsTrue(speakResult);
                }

                [Test]
                public void It_should_bind_the_choice_event () {
                    var choiceResult = false;
                    _ctrl.Events.Choice.AddListener((x, y, z) => choiceResult = true);
                    var playback = new DialoguePlayback(A.Graph.Build(), null, new DialogueEvents());

                    _ctrl.PlayChild(playback);
                    playback.Events.Choice.Invoke(null, null, null);

                    Assert.IsTrue(choiceResult);
                }

                [Test]
                public void It_should_call_Next_on_the_parent_if_End_is_called () {
                    var playback = new DialoguePlayback(A.Graph.Build(), null, new DialogueEvents());

                    // Automatically calls End since the graph is empty
                    _ctrl.PlayChild(playback);

                    _parentPlayback.Received(1).Next();
                }
            }
        }

        public class ActiveDialogueProperty : DialogueControllerTest {
            [Test]
            public void It_should_return_the_playing_graph () {
                _ctrl.Play(_playback);

                Assert.AreEqual(_playback, _ctrl.ActiveDialogue);
            }

            [Test]
            public void It_should_clear_if_playing_graph_calls_end () {
                var dialogueEmpty = new DialoguePlayback(A.Graph.Build(), null, new DialogueEvents());

                _ctrl.Play(dialogueEmpty);
                _ctrl.Next();

                Assert.AreEqual(null, _ctrl.ActiveDialogue);
            }

            [Test]
            public void It_should_return_PlayChild_after_play_is_called () {
                var dialogueChild = Substitute.For<IDialoguePlayback>();

                _ctrl.Play(_playback);
                _ctrl.PlayChild(dialogueChild);

                Assert.AreEqual(dialogueChild, _ctrl.ActiveDialogue);
            }

            [Test]
            public void It_should_return_Play_dialogue_if_PlayChild_dialogue_calls_end_event () {
                var dialogueEmpty = new DialoguePlayback(A.Graph.Build(), null, new DialogueEvents());

                _ctrl.Play(_playback);
                _ctrl.PlayChild(dialogueEmpty);
                _ctrl.Next();

                Assert.AreEqual(_playback, _ctrl.ActiveDialogue);
            }

            [Test]
            public void It_should_restore_the_parent_child_if_a_nested_child_ends () {
                var dialogueChild = Substitute.For<IDialoguePlayback>();
                var dialogueEmpty = new DialoguePlayback(A.Graph.Build(), null, new DialogueEvents());

                _ctrl.Play(_playback);
                _ctrl.PlayChild(dialogueChild);
                _ctrl.PlayChild(dialogueEmpty);
                _ctrl.Next();

                Assert.AreEqual(dialogueChild, _ctrl.ActiveDialogue);
            }
        }

        public class NextMethod : DialogueControllerTest {
            [Test]
            public void It_should_do_nothing_if_no_current_playback () {
                Assert.DoesNotThrow(() => _ctrl.Next());
            }

            [Test]
            public void It_should_call_Next_on_current_playback () {
                _ctrl.Play(_playback);
                _ctrl.Next();

                _playback.Received(1).Next();
            }
        }

        public class TickMethod : DialogueControllerTest {
            [Test]
            public void It_should_do_nothing_if_no_current_playback () {
                Assert.DoesNotThrow(() => _ctrl.Tick());
            }

            [Test]
            public void It_should_call_Tick_on_current_playback () {
                _ctrl.Play(_playback);
                _ctrl.Tick();

                _playback.Received(1).Tick();
            }
        }

        public class SelectChoice : DialogueControllerTest {
            [Test]
            public void It_should_do_nothing_if_no_current_playback () {
                Assert.DoesNotThrow(() => _ctrl.SelectChoice(0));
            }

            [Test]
            public void It_should_call_SelectChoice_on_current_playback () {
                _ctrl.Play(_playback);
                _ctrl.SelectChoice(0);

                _playback.Received(1).SelectChoice(0);
            }
        }

        public class StopMethod : DialogueControllerTest {
            [Test]
            public void It_should_do_nothing_if_no_current_playback () {
                Assert.DoesNotThrow(() => _ctrl.Stop());
            }

            [Test]
            public void It_should_run_stop_on_active_dialogue () {
                _ctrl.Play(_playback);
                _ctrl.Stop();

                _playback.Received(1).Stop();
            }

            [Test]
            public void It_should_run_stop_on_multiple_active_dialogues () {
                var playbackChild = Substitute.For<IDialoguePlayback>();

                _ctrl.Play(_playback);
                _ctrl.PlayChild(playbackChild);
                _ctrl.Stop();

                _playback.Received(1).Stop();
                playbackChild.Received(1).Stop();
            }

            [Test]
            public void It_should_clear_ActiveDialogue () {
                _ctrl.Play(_playback);
                _ctrl.Stop();

                Assert.IsNull(_ctrl.ActiveDialogue);
            }
        }
    }
}
