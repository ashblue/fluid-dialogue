using CleverCrow.Fluid.Dialogues.Conditions;
using NUnit.Framework;

namespace FluidDialogue.Tests.Editor {
    public class ConditionRuntimeTest {
        public class GetIsValidMethod {
            [Test]
            public void It_should_return_the_OnGetIsValid_value () {
                var condition = new ConditionRuntime(null) {
                    OnGetIsValid = () => true,
                };

                var result = condition.GetIsValid();

                Assert.IsTrue(result);
            }
        }
    }
}
