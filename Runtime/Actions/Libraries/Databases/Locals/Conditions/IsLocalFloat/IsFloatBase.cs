using System;
using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.Dialogues.Conditions;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    [CreateMenu("Database/Locals/Is Float")]
    public abstract class IsFloatBase : ConditionDataBase {
        private ConditionFloatInternal _condition;

        [SerializeField]
        private KeyValueDefinitionFloat _variable = null;

        [SerializeField]
        private NumberComparison _comparison = NumberComparison.Equal;

        [SerializeField]
        private float _value = 0;

        protected abstract IKeyValueData<float> GetFloatInstance (IDialogueController dialogue);

        public override void OnInit (IDialogueController dialogue) {
            _condition = new ConditionFloatInternal(GetFloatInstance(dialogue));
        }

        public override bool OnGetIsValid (INode parent) {
            return _condition.IsComparisonValid(_variable, _value, _comparison);
        }
    }

    public class ConditionFloatInternal {
        private readonly IKeyValueData<float> _database;

        public ConditionFloatInternal (IKeyValueData<float> database) {
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
