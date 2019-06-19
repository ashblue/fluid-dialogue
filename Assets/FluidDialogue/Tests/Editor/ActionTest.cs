using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Actions {
    public class ActionTest {
        private ActionExample _action;

        private class ActionExample : ActionBase {
            public int startCount;
            public int initCount;

            protected override void OnInit () {
                initCount += 1;
            }

            protected override void OnStart () {
                startCount += 1;
            }
        }

        [SetUp]
        public void BeforeEach () {
            _action = new ActionExample();
        }

        public class TickMethod {
            public void It_should_trigger_init () {

            }

            public void It_should_trigger_start () {

            }
        }

        public class InitMethod : ActionTest {
            [Test]
            public void It_should_trigger_init () {
                _action.Init();

                Assert.AreEqual(1, _action.initCount);
            }

            [Test]
            public void It_should_trigger_init_only_once () {
                _action.Init();
                _action.Init();

                Assert.AreEqual(1, _action.initCount);
            }
        }

        public class StartMethod : ActionTest {
            [Test]
            public void It_should_trigger_start () {
                _action.Start();

                Assert.AreEqual(1, _action.startCount);
            }

            [Test]
            public void It_should_not_trigger_start_again () {
                _action.Start();
                _action.Start();

                Assert.AreEqual(1, _action.startCount);
            }

            [Test]
            public void It_should_trigger_start_after_exit_is_called () {
                _action.Start();
                _action.Exit();
                _action.Start();

                Assert.AreEqual(2, _action.startCount);
            }
        }
    }
}
