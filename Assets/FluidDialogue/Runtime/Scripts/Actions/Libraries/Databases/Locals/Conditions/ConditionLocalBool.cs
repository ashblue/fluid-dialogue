using System;
using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.Dialogues.Conditions;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    public class ConditionLocalBool : ConditionDataBase {
        private ConditionLocalBoolInternal _condition;

        [SerializeField]
        private KeyValueDefinitionBool _variable = null;

        [SerializeField]
        private Comparison _comparison = Comparison.Equal;

        [SerializeField]
        private bool _value = true;

        private enum Comparison {
            Equal,
            NotEqual
        }

        protected override void OnInit (IDialogueController dialogue) {
            _condition = new ConditionLocalBoolInternal(dialogue.LocalDatabase.Bools);
        }

        protected override bool OnGetIsValid () {
            switch (_comparison) {
                case Comparison.Equal:
                    return _condition.AreValuesEqual(_variable, _value);
                case Comparison.NotEqual:
                    return _condition.AreValuesNotEqual(_variable, _value);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class ConditionLocalBoolInternal {
        private readonly IKeyValueData<bool> _database;

        public ConditionLocalBoolInternal (IKeyValueData<bool> database) {
            _database = database;
        }

        public bool AreValuesEqual (IKeyValueDefinition<bool> definition, bool value) {
            var kvp = _database.Get(definition.Key, definition.DefaultValue);

            return kvp == value;
        }

        public bool AreValuesNotEqual (IKeyValueDefinition<bool> definition, bool value) {
            var kvp = _database.Get(definition.Key, definition.DefaultValue);

            return kvp != value;
        }
    }
}
