using CleverCrow.Fluid.Databases;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    public class ConditionLocalBoolTest {
        private ConditionBoolInternal _boolCondition;
        private IKeyValueDefinition<bool> _definition;
        private IKeyValueData<bool> _database;

        [SetUp]
        public void BeforeEach () {
            _definition = Substitute.For<IKeyValueDefinition<bool>>();
            _database = Substitute.For<IKeyValueData<bool>>();
            _boolCondition = new ConditionBoolInternal(_database);

            _database.Get(null).ReturnsForAnyArgs(true);
        }

        public class AreValuesEqualMethod : ConditionLocalBoolTest {
            [Test]
            public void It_should_return_true_if_variable_and_value_are_equal () {
                var result = _boolCondition.AreValuesEqual(_definition, true);

                Assert.IsTrue(result);
            }

            [Test]
            public void It_should_return_false_if_variable_and_value_are_not_equal () {
                var result = _boolCondition.AreValuesEqual(_definition, false);

                Assert.IsFalse(result);
            }
        }

        public class AreValuesNotEqualMethod : ConditionLocalBoolTest {
            [Test]
            public void It_should_return_true_if_variable_and_value_are_not_equal () {
                var result = _boolCondition.AreValuesNotEqual(_definition, false);

                Assert.IsTrue(result);
            }

            [Test]
            public void It_should_return_false_if_variable_and_value_are_equal () {
                var result = _boolCondition.AreValuesNotEqual(_definition, true);

                Assert.IsFalse(result);
            }
        }
    }
}
