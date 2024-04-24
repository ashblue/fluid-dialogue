using CleverCrow.Fluid.Dialogues.Conditions;
using NSubstitute;
using NUnit.Framework;

namespace FluidDialogue.Tests.Editor {
    public class ConditionRuntimeTest {
        private IConditionData _data;

        [SetUp]
        public void Setup () {
            _data = Substitute.For<IConditionData>();
        }

        public class GetIsValidMethod {
            public class OnGetIsValidTriggering : ConditionRuntimeTest {
                [Test]
                public void It_should_return_the_OnGetIsValid_value () {
                    _data.OnGetIsValid(null).Returns(true);
                    var condition = new ConditionRuntime(null, null, _data);

                    var result = condition.GetIsValid(null);

                    Assert.IsTrue(result);
                }
            }

            public class OnInitTriggering : ConditionRuntimeTest {
                [Test]
                public void It_should_trigger_OnInit_with_a_dialogue_controller () {
                    var condition = new ConditionRuntime(null, null, _data);

                    condition.GetIsValid(null);

                    _data.Received(1).OnInit(null);
                }

                [Test]
                public void It_should_trigger_OnInit_only_once () {
                    var condition = new ConditionRuntime(null, null, _data);

                    condition.GetIsValid(null);
                    condition.GetIsValid(null);

                    _data.Received(1).OnInit(null);
                }
            }
        }
    }
}
