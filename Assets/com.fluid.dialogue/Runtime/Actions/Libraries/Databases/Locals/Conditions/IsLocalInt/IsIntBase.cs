using System;
using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.Dialogues.Conditions;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    public abstract class IsIntBase : ConditionDataBase {
        private ConditionIntInternal _condition;

        [SerializeField]
        private KeyValueDefinitionInt _variable = null;

        [SerializeField]
        private NumberComparison _comparison = NumberComparison.Equal;

        [SerializeField]
        private int _value = 0;

        protected abstract IKeyValueData<int> GetIntInstance (IDialogueController dialogue);

        public override void OnInit (IDialogueController dialogue) {
            _condition = new ConditionIntInternal(GetIntInstance(dialogue));
        }

        public override bool OnGetIsValid (INode parent) {
            return _condition.IsComparisonValid(_variable, _value, _comparison);
        }
    }

    public class ConditionIntInternal {
        private readonly IKeyValueData<int> _database;

        public ConditionIntInternal (IKeyValueData<int> database) {
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
