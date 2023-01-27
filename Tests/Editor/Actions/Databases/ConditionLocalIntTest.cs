using CleverCrow.Fluid.Databases;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    public class ConditionLocalIntTest {
        private ConditionIntInternal _condition;
        private IKeyValueDefinition<int> _definition;
        private IKeyValueData<int> _database;

        [SetUp]
        public void BeforeEach () {
            _definition = Substitute.For<IKeyValueDefinition<int>>();
            _database = Substitute.For<IKeyValueData<int>>();
            _condition = new ConditionIntInternal(_database);

            _database.Get(null).ReturnsForAnyArgs(0);
        }

        public class IsComparisonValidMethod {
            public class ComparisonEqualEnum : ConditionLocalIntTest {
                [Test]
                public void It_should_return_true_for_equal () {
                    var result = _condition.IsComparisonValid(_definition, 0, NumberComparison.Equal);

                    Assert.IsTrue(result);
                }

                [Test]
                public void It_should_return_false_for_not_equal () {
                    var result = _condition.IsComparisonValid(_definition, 1, NumberComparison.Equal);

                    Assert.IsFalse(result);
                }
            }

            public class ComparisonNotEqualEnum : ConditionLocalIntTest {
                [Test]
                public void It_should_return_true_for_not_equal () {
                    var result = _condition.IsComparisonValid(_definition, 1, NumberComparison.NotEqual);

                    Assert.IsTrue(result);
                }

                [Test]
                public void It_should_return_false_for_equal () {
                    var result = _condition.IsComparisonValid(_definition, 0, NumberComparison.NotEqual);

                    Assert.IsFalse(result);
                }
            }

            public class ComparisonGreaterThanEnum : ConditionLocalIntTest {
                [Test]
                public void It_should_return_true_if_greater_than () {
                    var result = _condition.IsComparisonValid(_definition, 1, NumberComparison.GreaterThan);

                    Assert.IsTrue(result);
                }

                [Test]
                public void It_should_return_false_if_equal () {
                    var result = _condition.IsComparisonValid(_definition, 0, NumberComparison.GreaterThan);

                    Assert.IsFalse(result);
                }
            }

            public class ComparisonLessThanEnum : ConditionLocalIntTest {
                [Test]
                public void It_should_return_true_if_less_than () {
                    var result = _condition.IsComparisonValid(_definition, -1, NumberComparison.LessThan);

                    Assert.IsTrue(result);
                }

                [Test]
                public void It_should_return_false_if_equal () {
                    var result = _condition.IsComparisonValid(_definition, 0, NumberComparison.LessThan);

                    Assert.IsFalse(result);
                }
            }

            public class ComparisonGreaterThanOrEqualEnum : ConditionLocalIntTest {
                [Test]
                public void It_should_return_true_if_greater_than () {
                    var result = _condition.IsComparisonValid(_definition, 1, NumberComparison.GreaterThanOrEqual);

                    Assert.IsTrue(result);
                }

                [Test]
                public void It_should_return_true_if_equal () {
                    var result = _condition.IsComparisonValid(_definition, 0, NumberComparison.GreaterThanOrEqual);

                    Assert.IsTrue(result);
                }

                [Test]
                public void It_should_return_false_if_less_than () {
                    var result = _condition.IsComparisonValid(_definition, -1, NumberComparison.GreaterThanOrEqual);

                    Assert.IsFalse(result);
                }
            }

            public class ComparisonLessThanOrEqualEnum : ConditionLocalIntTest {
                [Test]
                public void It_should_return_true_if_less_than () {
                    var result = _condition.IsComparisonValid(_definition, -1, NumberComparison.LessThanOrEqual);

                    Assert.IsTrue(result);
                }

                [Test]
                public void It_should_return_true_if_equal () {
                    var result = _condition.IsComparisonValid(_definition, 0, NumberComparison.LessThanOrEqual);

                    Assert.IsTrue(result);
                }

                [Test]
                public void It_should_return_false_if_greater_than () {
                    var result = _condition.IsComparisonValid(_definition, 1, NumberComparison.LessThanOrEqual);

                    Assert.IsFalse(result);
                }
            }
        }
    }
}
