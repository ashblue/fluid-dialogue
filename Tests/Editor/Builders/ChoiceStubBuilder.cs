using CleverCrow.Fluid.Dialogues.Choices;
using CleverCrow.Fluid.Dialogues.Nodes;
using NSubstitute;

namespace CleverCrow.Fluid.Dialogues.Builders {
    public class ChoiceStubBuilder {
        private bool _overrideChildNode;
        private INode _validChildNode;

        private bool _isValid = true;

        public ChoiceStubBuilder WithIsValid (bool isValid) {
            _isValid = isValid;
            return this;
        }

        public ChoiceStubBuilder WithValidChildNode (INode validChildNode) {
            _overrideChildNode = true;
            _validChildNode = validChildNode;
            return this;
        }

        public IChoice Build () {
            var choice = Substitute.For<IChoice>();
            choice.IsValid.Returns(_isValid);

            if (_overrideChildNode) {
                choice.GetValidChildNode().Returns(_validChildNode);
            }

            return choice;
        }
    }
}
