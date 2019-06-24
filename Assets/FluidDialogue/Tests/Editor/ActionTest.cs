using System.Threading.Tasks;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Actions {
    public class ActionTest {
        private ActionRuntime _action;

        [SetUp]
        public void BeforeEach () {
            _action = new ActionRuntime(null) {
                OnUpdate = () => ActionStatus.Continue
            };
        }

        public class TickMethod {
            public class OnInitTriggering : ActionTest {
                [Test]
                public void It_should_trigger_OnInit () {
                    var initCount = 0;
                    _action.OnInit = () => initCount += 1;

                    _action.Tick();

                    Assert.AreEqual(1, initCount);
                }

                [Test]
                public void It_should_trigger_OnInit_only_once () {
                    var initCount = 0;
                    _action.OnInit = () => initCount += 1;

                    _action.Tick();
                    _action.Tick();

                    Assert.AreEqual(1, initCount);
                }
            }

            public class OnStartTriggering : ActionTest {
                private int _startCount;

                [SetUp]
                public void BeforeEachMethod () {
                    _startCount = 0;
                    _action.OnStart = () => _startCount += 1;
                }

                [Test]
                public void It_should_trigger_OnStart () {
                    _action.Tick();

                    Assert.AreEqual(1, _startCount);
                }

                [Test]
                public void It_should_not_trigger_OnStart_again () {
                    _action.Tick();
                    _action.Tick();

                    Assert.AreEqual(1, _startCount);
                }

                [Test]
                public void It_should_trigger_OnStart_after_OnUpdate_returns_success () {
                    _action.Tick();
                    _action.OnUpdate = () => ActionStatus.Success;
                    _action.Tick();
                    _action.Tick();

                    Assert.AreEqual(2, _startCount);
                }
            }

            public class OnUpdateTriggering : ActionTest {
                [Test]
                public void It_should_trigger_OnUpdate () {
                    var updateCount = 0;
                    _action.OnUpdate = () => {
                        updateCount += 1;
                        return ActionStatus.Success;
                    };

                    _action.Tick();

                    Assert.AreEqual(1, updateCount);
                }

                [Test]
                public void It_should_return_the_update_status () {
                    _action.OnUpdate = () => ActionStatus.Continue;

                    var status = _action.Tick();

                    Assert.AreEqual(ActionStatus.Continue, status);
                }
            }

            public class OnExitTriggering : ActionTest {
                [Test]
                public void It_should_trigger_OnExit_if_OnUpdate_returns_success () {
                    var exitCount = 0;
                    _action.OnUpdate = () => ActionStatus.Success;
                    _action.OnExit = () => exitCount += 1;

                    _action.Tick();

                    Assert.AreEqual(1, exitCount);
                }

                [Test]
                public void It_should_not_trigger_OnExit_if_OnUpdate_returns_continue () {
                    var exitCount = 0;
                    _action.OnUpdate = () => ActionStatus.Continue;
                    _action.OnExit = () => exitCount += 1;

                    _action.Tick();

                    Assert.AreEqual(0, exitCount);
                }
            }

            public class OnResetTriggering : ActionTest {
                [Test]
                public void It_should_trigger_reset_after_OnUpdate_returns_success () {
                    var resetCount = 0;
                    _action.OnReset = () => resetCount += 1;
                    _action.OnUpdate = () => ActionStatus.Success;

                    _action.Tick();
                    _action.Tick();

                    Assert.AreEqual(1, resetCount);
                }

                [Test]
                public void It_should_not_trigger_reset_after_OnUpdate_returns_continue () {
                    var resetCount = 0;
                    _action.OnReset = () => resetCount += 1;
                    _action.OnUpdate = () => ActionStatus.Continue;

                    _action.Tick();
                    _action.Tick();

                    Assert.AreEqual(0, resetCount);
                }
            }

            public class EndMethod : ActionTest {
                [Test]
                public void It_should_not_call_OnExit_if_Tick_returns_success () {
                    var exitCount = 0;
                    _action.OnUpdate = () => ActionStatus.Success;

                    _action.Tick();
                    _action.OnExit = () => exitCount += 1;
                    _action.End();

                    Assert.AreEqual(0, exitCount);
                }

                [Test]
                public void It_should_call_OnExit_if_Tick_returned_continue () {
                    var exitCount = 0;
                    _action.OnUpdate = () => ActionStatus.Continue;

                    _action.Tick();
                    _action.OnExit = () => exitCount += 1;
                    _action.End();

                    Assert.AreEqual(1, exitCount);
                }

                [Test]
                public void It_should_not_call_OnExit_if_Tick_has_not_been_called () {
                    var exitCount = 0;
                    _action.OnExit = () => exitCount += 1;

                    _action.End();

                    Assert.AreEqual(0, exitCount);
                }
            }
        }
    }
}
