using CleverCrow.Fluid.Databases;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    public class ConditionLocalStringTest {
        private const string VALUE = "a";

        private ConditionStringInternal _condition;
        private IKeyValueDefinition<string> _definition;
        private IKeyValueData<string> _database;

        [SetUp]
        public void BeforeEach () {
            _definition = Substitute.For<IKeyValueDefinition<string>>();
            _database = Substitute.For<IKeyValueData<string>>();
            _condition = new ConditionStringInternal(_database);

            _database.Get(null).ReturnsForAnyArgs(VALUE);
        }

        public class AreValuesEqualMethod : ConditionLocalStringTest {
            [Test]
            public void It_should_return_true_if_variable_and_value_are_equal () {
                var result = _condition.AreValuesEqual(_definition, VALUE);

                Assert.IsTrue(result);
            }

            [Test]
            public void It_should_return_false_if_variable_and_value_are_not_equal () {
                var result = _condition.AreValuesEqual(_definition, "b");

                Assert.IsFalse(result);
            }
        }

        public class AreValuesNotEqualMethod : ConditionLocalStringTest {
            [Test]
            public void It_should_return_true_if_variable_and_value_are_not_equal () {
                var result = _condition.AreValuesNotEqual(_definition, "b");

                Assert.IsTrue(result);
            }

            [Test]
            public void It_should_return_false_if_variable_and_value_are_equal () {
                var result = _condition.AreValuesNotEqual(_definition, VALUE);

                Assert.IsFalse(result);
            }
        }
    }
}
