using System;
using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.Dialogues.Conditions;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    [CreateMenu("Database/Locals/Is Int")]
    public class IsLocalInt : ConditionDataBase {
        private ConditionLocalIntInternal _condition;

        [SerializeField]
        private KeyValueDefinitionInt _variable = null;

        [SerializeField]
        private NumberComparison _comparison = NumberComparison.Equal;

        [SerializeField]
        private int _value = 0;

        public override void OnInit (IDialogueController dialogue) {
            _condition = new ConditionLocalIntInternal(dialogue.LocalDatabase.Ints);
        }

        public override bool OnGetIsValid () {
            return _condition.IsComparisonValid(_variable, _value, _comparison);
        }
    }

    public class ConditionLocalIntInternal {
        private readonly IKeyValueData<int> _database;

        public ConditionLocalIntInternal (IKeyValueData<int> database) {
            _database = database;
        }

        public bool IsComparisonValid (IKeyValueDefinition<int> definition, int value, NumberComparison comparison) {
            var dbValue = _database.Get(definition.Key, definition.DefaultValue);

            switch (comparison) {
                case NumberComparison.Equal:
                    return dbValue == value;
                case NumberComparison.NotEqual:
                    return dbValue != value;
                case NumberComparison.GreaterThan:
                    return value > dbValue;
                case NumberComparison.GreaterThanOrEqual:
                    return value >= dbValue;
                case NumberComparison.LessThan:
                    return value < dbValue;
                case NumberComparison.LessThanOrEqual:
                    return value <= dbValue;
                default:
                    throw new ArgumentOutOfRangeException(nameof(comparison), comparison, null);
            }
        }
    }
}
