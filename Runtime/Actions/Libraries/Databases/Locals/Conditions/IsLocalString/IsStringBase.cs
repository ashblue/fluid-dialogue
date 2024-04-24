using System;
using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.Dialogues.Conditions;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    public abstract class IsStringBase : ConditionDataBase {
        private ConditionStringInternal _condition;

        [SerializeField]
        private KeyValueDefinitionString _variable = null;

        [SerializeField]
        private Comparison _comparison = Comparison.Equal;

        [SerializeField]
        private string _value = null;

        private enum Comparison {
            Equal,
            NotEqual
        }

        protected abstract IKeyValueData<string> GetStringInstance (IDialogueController dialogue);

        public override void OnInit (IDialogueController dialogue) {
            _condition = new ConditionStringInternal(GetStringInstance(dialogue));
        }

        public override bool OnGetIsValid (INode parent) {
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

    public class ConditionStringInternal {
        private readonly IKeyValueData<string> _database;

        public ConditionStringInternal (IKeyValueData<string> database) {
            _database = database;
        }

        public bool AreValuesEqual (IKeyValueDefinition<string> definition, string value) {
            var kvp = _database.Get(definition.Key, definition.DefaultValue);

            return kvp == value;
        }

        public bool AreValuesNotEqual (IKeyValueDefinition<string> definition, string value) {
            var kvp = _database.Get(definition.Key, definition.DefaultValue);

            return kvp != value;
        }
    }
}
