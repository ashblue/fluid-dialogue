using CleverCrow.Fluid.Dialogues.Actions;
using NSubstitute;

namespace CleverCrow.Fluid.Dialogues.Builders {
    public class ActionStubBuilder {
        private ActionStatus _tickStatus = ActionStatus.Success;

        public ActionStubBuilder WithTickStatus (ActionStatus status) {
            _tickStatus = status;
            return this;
        }

        public IAction Build () {
            var action = Substitute.For<IAction>();
            action.Tick().Returns(_tickStatus);

            return action;
        }
    }
}
