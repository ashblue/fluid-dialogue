using System;
using CleverCrow.Fluid.Dialogues.Builders;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues {
    public class DialogueControllerTest {
        private DialogueController _ctrl;

        [SetUp]
        public void BeforeEach () {
            _ctrl = new DialogueController();
        }

        public class PlayMethod : DialogueControllerTest {
            [Test]
            public void It_should_run_play_on_the_graph () {
                var playback = Substitute.For<IDialoguePlayback>();

                _ctrl.Play(playback);

                playback.Received(1).Play();
            }

            [Test]
            public void It_should_bind_the_begin_event () {
                var beginResult = false;
                _ctrl.Events.Begin.AddListener(() => beginResult = true);
                var playback = new DialoguePlayback(A.Graph.Build(), new DialogueEvents());

                _ctrl.Play(playback);

                Assert.IsTrue(beginResult);
            }

            [Test]
            public void It_should_bind_the_end_event () {
                var endResult = false;
                _ctrl.Events.End.AddListener(() => endResult = true);
                var playbackEmpty = new DialoguePlayback(A.Graph.Build(), new DialogueEvents());

                _ctrl.Play(playbackEmpty);

                Assert.IsTrue(endResult);
            }

            [Test]
            public void It_should_bind_dialogue_speak_events () {
                var speakResult = false;
                _ctrl.Events.Speak.AddListener((x, y) => speakResult = true);
                var playback = new DialoguePlayback(A.Graph.Build(), new DialogueEvents());

                _ctrl.Play(playback);
                playback.Events.Speak.Invoke(null, null);

                Assert.IsTrue(speakResult);
            }

            [Test]
            public void It_should_bind_dialogue_choice_events () {
                var choiceResult = false;
                _ctrl.Events.Choice.AddListener((x, y, z) => choiceResult = true);
                var playback = new DialoguePlayback(A.Graph.Build(), new DialogueEvents());

                _ctrl.Play(playback);
                playback.Events.Choice.Invoke(null, null, null);

                Assert.IsTrue(choiceResult);
            }
        }

        public class PlayChildMethod {
            public class ErrorHandling : DialogueControllerTest {
                [Test]
                public void It_should_throw_an_error_if_nothing_is_playing () {
                    var playback = Substitute.For<IDialoguePlayback>();

                    Assert.Throws<InvalidOperationException>(
                        () => _ctrl.PlayChild(playback), "Cannot trigger child dialogue, nothing is playing");
                }
            }

            public class SuccessfulRuns : DialogueControllerTest {
                private IDialoguePlayback _parentPlayback;

                [SetUp]
                public void BeforeEachMethod () {
                    _parentPlayback = Substitute.For<IDialoguePlayback>();
                    _ctrl.Play(_parentPlayback);
                }

                [Test]
                public void It_should_call_Play_on_the_dialogue () {
                    var playback = Substitute.For<IDialoguePlayback>();

                    _ctrl.PlayChild(playback);

                    playback.Received(1).Play();
                }

                [Test]
                public void It_should_bind_the_speak_event () {
                    var speakResult = false;
                    _ctrl.Events.Speak.AddListener((x, y) => speakResult = true);
                    var playback = new DialoguePlayback(A.Graph.Build(), new DialogueEvents());

                    _ctrl.PlayChild(playback);
                    playback.Events.Speak.Invoke(null, null);

                    Assert.IsTrue(speakResult);
                }

                [Test]
                public void It_should_bind_the_choice_event () {
                    var choiceResult = false;
                    _ctrl.Events.Choice.AddListener((x, y, z) => choiceResult = true);
                    var playback = new DialoguePlayback(A.Graph.Build(), new DialogueEvents());

                    _ctrl.PlayChild(playback);
                    playback.Events.Choice.Invoke(null, null, null);

                    Assert.IsTrue(choiceResult);
                }

                [Test]
                public void It_should_call_Next_on_the_parent_if_End_is_called () {
                    var playback = new DialoguePlayback(A.Graph.Build(), new DialogueEvents());

                    // Automatically calls End since the graph is empty
                    _ctrl.PlayChild(playback);

                    _parentPlayback.Received(1).Next();
                }
            }
        }

        public class ActiveDialogueProperty : DialogueControllerTest {
            [Test]
            public void It_should_return_the_playing_graph () {
                var dialogue = Substitute.For<IDialoguePlayback>();

                _ctrl.Play(dialogue);

                Assert.AreEqual(dialogue, _ctrl.ActiveDialogue);
            }

            [Test]
            public void It_should_clear_if_playing_graph_calls_end () {
                var dialogueEmpty = new DialoguePlayback(A.Graph.Build(), new DialogueEvents());

                _ctrl.Play(dialogueEmpty);

                Assert.AreEqual(null, _ctrl.ActiveDialogue);
            }

            [Test]
            public void It_should_return_PlayChild_after_play_is_called () {
                var dialogue = Substitute.For<IDialoguePlayback>();
                var dialogueChild = Substitute.For<IDialoguePlayback>();

                _ctrl.Play(dialogue);
                _ctrl.PlayChild(dialogueChild);

                Assert.AreEqual(dialogueChild, _ctrl.ActiveDialogue);
            }

            [Test]
            public void It_should_return_Play_dialogue_if_PlayChild_dialogue_calls_end_event () {
                var dialogue = Substitute.For<IDialoguePlayback>();
                var dialogueEmpty = new DialoguePlayback(A.Graph.Build(), new DialogueEvents());

                _ctrl.Play(dialogue);
                _ctrl.PlayChild(dialogueEmpty);

                Assert.AreEqual(dialogue, _ctrl.ActiveDialogue);
            }

            [Test]
            public void It_should_restore_the_parent_child_if_a_nest_child_ends () {
                var dialogue = Substitute.For<IDialoguePlayback>();
                var dialogueChild = Substitute.For<IDialoguePlayback>();
                var dialogueEmpty = new DialoguePlayback(A.Graph.Build(), new DialogueEvents());

                _ctrl.Play(dialogue);
                _ctrl.PlayChild(dialogueChild);
                _ctrl.PlayChild(dialogueEmpty);

                Assert.AreEqual(dialogueChild, _ctrl.ActiveDialogue);
            }
        }

        public class NextMethod {
            public void It_should_do_nothing_if_no_current_playback () {

            }

            public void It_should_call_Next_on_current_playback () {

            }
        }

        public class TickMethod {
            public void It_should_do_nothing_if_no_current_playback () {

            }

            public void It_should_call_Tick_on_current_playback () {

            }
        }

        public class SelectChoice {
            public void It_should_do_nothing_if_no_current_playback () {

            }

            public void It_should_call_SelectChoice_on_current_playback () {

            }
        }

        public class StopMethod {
            public void It_should_do_nothing_if_no_current_playback () {

            }

            public void It_should_run_stop_on_all_active_dialogue_playbacks () {

            }

            public void It_should_clear_ActiveDialogue () {

            }
        }
    }
}
