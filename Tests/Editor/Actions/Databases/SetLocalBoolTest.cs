using CleverCrow.Fluid.Databases;
using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    public class SetLocalBoolTest {
        public class WriteValueMethod {
            [Test]
            public void It_should_set_the_local_database_value () {
                const string KEY = "key";
                const bool VALUE = true;

                var database = Substitute.For<IKeyValueData<bool>>();
                var setter = new SetKeyValueInternal<bool>(database);

                setter.WriteValue(KEY, VALUE);

                database.Received(1).Set(KEY, VALUE);
            }
        }
    }
}
