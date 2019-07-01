using CleverCrow.Fluid.Dialogues.Conditions;
using NUnit.Framework;

namespace FluidDialogue.Tests.Editor {
    public class ConditionRuntimeTest {
        public class GetIsValidMethod {
            public class OnGetIsValidTriggering {
                [Test]
                public void It_should_return_the_OnGetIsValid_value () {
                    var condition = new ConditionRuntime(null, null) {
                        OnGetIsValid = () => true,
                    };

                    var result = condition.GetIsValid();

                    Assert.IsTrue(result);
                }
            }

            public class OnInitTriggering {
                [Test]
                public void It_should_trigger_OnInit_with_a_dialogue_controller () {
                    var runCount = 0;
                    var condition = new ConditionRuntime(null, null) {
                        OnInit = (dialogue) => runCount++,
                    };

                    condition.GetIsValid();

                    Assert.AreEqual(1, runCount);
                }

                [Test]
                public void It_should_trigger_OnInit_only_once () {
                    var runCount = 0;
                    var condition = new ConditionRuntime(null, null) {
                        OnInit = (dialogue) => runCount++,
                    };

                    condition.GetIsValid();
                    condition.GetIsValid();

                    Assert.AreEqual(1, runCount);
                }
            }
        }
    }
}
