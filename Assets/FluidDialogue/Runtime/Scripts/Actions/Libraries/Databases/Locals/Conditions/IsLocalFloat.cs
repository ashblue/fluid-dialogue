using System;
using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.Dialogues.Conditions;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    [CreateMenu("Database/Locals/Is Float")]
    public class ConditionLocalFloat : ConditionDataBase {
        private ConditionLocalFloatInternal _condition;

        [SerializeField]
        private KeyValueDefinitionFloat _variable = null;

        [SerializeField]
        private NumberComparison _comparison = NumberComparison.Equal;

        [SerializeField]
        private float _value = 0;

        public override void OnInit (IDialogueController dialogue) {
            _condition = new ConditionLocalFloatInternal(dialogue.LocalDatabase.Floats);
        }

        public override bool OnGetIsValid () {
            return _condition.IsComparisonValid(_variable, _value, _comparison);
        }
    }

    public class ConditionLocalFloatInternal {
        private readonly IKeyValueData<float> _database;

        public ConditionLocalFloatInternal (IKeyValueData<float> database) {
            _database = database;
        }

        public bool IsComparisonValid (IKeyValueDefinition<float> definition, float value,
            NumberComparison comparison) {
            var dbValue = _database.Get(definition.Key, definition.DefaultValue);

            switch (comparison) {
                case NumberComparison.Equal:
                    return Math.Abs(dbValue - value) < 0.01f;
                case NumberComparison.NotEqual:
                    return Math.Abs(dbValue - value) > 0.01f;
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
