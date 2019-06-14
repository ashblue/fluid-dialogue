using CleverCrow.Fluid.Dialogues.Builders;
using CleverCrow.Fluid.Dialogues.Graphs;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues {
    public class DialoguePlaybackTest {
        private DialoguePlayback _playback;
        private IDialogueGraph _graph;

        [SetUp]
        public void BeforeEach () {
            _graph = A.Graph()
                .WithNextResult(A.Node().Build())
                .Build();

            var events = Substitute.For<IDialoguePlaybackEvents>();
            _playback = new DialoguePlayback(events);
        }

        public class PlayMethod : DialoguePlaybackTest {
            [Test]
            public void It_should_trigger_next_on_the_root_node () {
                _playback.Play(_graph);

                _graph.Root.Received(1).Next();
            }

            [Test]
            public void It_should_trigger_a_Begin_event () {
                _playback.Play(_graph);

                _playback.Events.Begin.Received(1).Invoke();
            }

            [Test]
            public void It_should_trigger_a_speak_event_with_the_root_child_dialogue () {
                var node = A.Node().Build();
                _graph = A.Graph()
                    .WithNextResult(node)
                    .Build();

                _playback.Play(_graph);

                _playback.Events.Speak.Received(1).Invoke(node.Actor, node.Dialogue);
            }

            [Test]
            public void If_no_child_on_root_do_not_trigger_begin_event () {
                _graph.Root.Next().Returns((x) => null);

                _playback.Play(_graph);

                _playback.Events.Begin.DidNotReceive().Invoke();
            }
        }

        public class NextMethod : DialoguePlaybackTest {
            [Test]
            public void It_should_trigger_speak_on_the_next_sibling_node () {
                var nodeNested = A.Node().Build();
                var node = A.Node()
                    .WithNextResult(nodeNested)
                    .Build();
                _graph = A.Graph()
                    .WithNextResult(node)
                    .Build();

                _playback.Play(_graph);
                _playback.Next();

                _playback.Events.Speak.Received(1).Invoke(nodeNested.Actor, nodeNested.Dialogue);
            }

            [Test]
            public void If_no_next_result_trigger_end_event () {
                var node = A.Node()
                    .WithNextResult(null)
                    .Build();
                _graph = A.Graph()
                    .WithNextResult(node)
                    .Build();

                _playback.Play(_graph);
                _playback.Next();

                _playback.Events.End.Received(1).Invoke();
            }
        }
    }
}
