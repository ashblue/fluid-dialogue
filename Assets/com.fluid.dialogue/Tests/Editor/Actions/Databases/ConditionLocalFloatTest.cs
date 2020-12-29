using CleverCrow.Fluid.Databases;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    public class ConditionLocalFloatTest {
        private ConditionFloatInternal _condition;
        private IKeyValueDefinition<float> _definition;
        private IKeyValueData<float> _database;

        [SetUp]
        public void BeforeEach () {
            _definition = Substitute.For<IKeyValueDefinition<float>>();
            _database = Substitute.For<IKeyValueData<float>>();
            _condition = new ConditionFloatInternal(_database);

            _database.Get(null).ReturnsForAnyArgs(0);
        }

        public class IsComparisonValidMethod {
            public class ComparisonEqualEnum : ConditionLocalFloatTest {
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

            public class ComparisonNotEqualEnum : ConditionLocalFloatTest {
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

            public class ComparisonGreaterThanEnum : ConditionLocalFloatTest {
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

            public class ComparisonLessThanEnum : ConditionLocalFloatTest {
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

            public class ComparisonGreaterThanOrEqualEnum : ConditionLocalFloatTest {
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

            public class ComparisonLessThanOrEqualEnum : ConditionLocalFloatTest {
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
