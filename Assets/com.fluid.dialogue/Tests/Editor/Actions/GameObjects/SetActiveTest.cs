using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Actions.GameObjects {
    public class SetActiveTest {
        public class SetValueMethod {
            [Test]
            public void It_should_set_the_GameObject_active_to_true () {
                var go = Substitute.For<IGameObjectWrapper>();

                var setActive = new SetActiveInternal(go);
                setActive.SetValue(true);

                go.Received(1).SetActive(true);
            }

            [Test]
            public void It_should_set_the_GameObject_active_to_false () {
                var go = Substitute.For<IGameObjectWrapper>();

                var setActive = new SetActiveInternal(go);
                setActive.SetValue(false);

                go.Received(1).SetActive(false);
            }
        }
    }
}
