using CleverCrow.Fluid.Databases;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    public abstract class SetLocalVariableBase<T> : ActionDataBase {
        private SetKeyValueInternal<T> _setKeyValue;

        [SerializeField]
        public T _value;

        protected abstract KeyValueDefinitionBase<T> Variable { get; }

        public override void OnInit (IDialogueController controller) {
            _setKeyValue = new SetKeyValueInternal<T>(GetDatabase(controller));
        }

        public override ActionStatus OnUpdate () {
            _setKeyValue.WriteValue(Variable.key, _value);

            return base.OnUpdate();
        }

        protected abstract IKeyValueData<T> GetDatabase (IDialogueController dialogue);
    }

    public class SetKeyValueInternal<T> {
        private readonly IKeyValueData<T> _database;

        public SetKeyValueInternal (IKeyValueData<T> database) {
            _database = database;
        }

        public void WriteValue (string key, T value) {
            _database.Set(key, value);
        }
    }
}
