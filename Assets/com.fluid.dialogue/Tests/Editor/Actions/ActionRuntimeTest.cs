using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Actions {
    public class ActionRuntimeTest {
        private IActionData _data;
        private IDialogueController _dialogue;
        private ActionRuntime _action;

        [SetUp]
        public void BeforeEach () {
            _data = Substitute.For<IActionData>();
            _data.OnUpdate().Returns(ActionStatus.Continue);

            _dialogue = Substitute.For<IDialogueController>();
            _action = new ActionRuntime(_dialogue, null, _data);
        }

        public class TickMethod {
            public class OnInitTriggering : ActionRuntimeTest {
                [Test]
                public void It_should_trigger_OnInit_with_a_dialogue_controller () {
                    _action.Tick();

                    _data.Received(1).OnInit(_dialogue);
                }

                [Test]
                public void It_should_trigger_OnInit_only_once () {
                    _action.Tick();
                    _action.Tick();

                    _data.Received(1).OnInit(_dialogue);
                }
            }

            public class OnStartTriggering : ActionRuntimeTest {
                [Test]
                public void It_should_trigger_OnStart () {
                    _action.Tick();

                    _data.Received(1).OnStart();
                }

                [Test]
                public void It_should_not_trigger_OnStart_again () {
                    _action.Tick();
                    _action.Tick();

                    _data.Received(1).OnStart();
                }

                [Test]
                public void It_should_trigger_OnStart_after_OnUpdate_returns_success () {
                    _action.Tick();
                    _data.OnUpdate().Returns(ActionStatus.Success);
                    _action.Tick();
                    _action.Tick();

                    _data.Received(2).OnStart();
                }
            }

            public class OnUpdateTriggering : ActionRuntimeTest {
                [Test]
                public void It_should_trigger_OnUpdate () {
                    _action.Tick();

                    _data.Received(1).OnUpdate();
                }

                [Test]
                public void It_should_return_the_update_status () {
                    _data.OnUpdate().Returns(ActionStatus.Continue);

                    var status = _action.Tick();

                    Assert.AreEqual(ActionStatus.Continue, status);
                }
            }

            public class OnExitTriggering : ActionRuntimeTest {
                [Test]
                public void It_should_trigger_OnExit_if_OnUpdate_returns_success () {
                    _data.OnUpdate().Returns(ActionStatus.Success);

                    _action.Tick();

                    _data.Received(1).OnExit();
                }

                [Test]
                public void It_should_not_trigger_OnExit_if_OnUpdate_returns_continue () {
                    _data.OnUpdate().Returns(ActionStatus.Continue);

                    _action.Tick();

                    _data.Received(0).OnExit();
                }
            }

            public class OnResetTriggering : ActionRuntimeTest {
                [Test]
                public void It_should_trigger_reset_after_OnUpdate_returns_success () {
                    _data.OnUpdate().Returns(ActionStatus.Success);

                    _action.Tick();
                    _action.Tick();

                    _data.Received(1).OnReset();
                }

                [Test]
                public void It_should_not_trigger_reset_after_OnUpdate_returns_continue () {
                    _data.OnUpdate().Returns(ActionStatus.Continue);

                    _action.Tick();
                    _action.Tick();

                    _data.Received(0).OnReset();
                }
            }

            public class EndMethod : ActionRuntimeTest {
                [Test]
                public void It_should_not_call_OnExit_if_Tick_returns_success () {
                    _data.OnUpdate().Returns(ActionStatus.Success);

                    _action.Tick();
                    _action.End();

                    _data.Received(1).OnExit();
                }

                [Test]
                public void It_should_call_OnExit_if_Tick_returned_continue () {
                    _data.OnUpdate().Returns(ActionStatus.Continue);

                    _action.Tick();
                    _action.End();

                    _data.Received(1).OnExit();
                }

                [Test]
                public void It_should_not_call_OnExit_if_Tick_has_not_been_called () {
                    _action.End();

                    _data.Received(0).OnExit();
                }
            }
        }
    }
}
