using System;
using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.Dialogues.Conditions;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    [CreateMenu("Database/Locals/Is String")]
    public class IsLocalString : ConditionDataBase {
        private ConditionLocalStringInternal _condition;

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

        public override void OnInit (IDialogueController dialogue) {
            _condition = new ConditionLocalStringInternal(dialogue.LocalDatabase.Strings);
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

    public class ConditionLocalStringInternal {
        private readonly IKeyValueData<string> _database;

        public ConditionLocalStringInternal (IKeyValueData<string> database) {
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
