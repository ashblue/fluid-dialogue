using CleverCrow.Fluid.Databases;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    [CreateMenu("Database/Locals/Set String")]
    public class SetLocalString : SetLocalVariableBase<string> {
        [SerializeField]
        public KeyValueDefinitionString _variable;

        protected override KeyValueDefinitionBase<string> Variable => _variable;

        protected override IKeyValueData<string> GetDatabase (IDialogueController dialogue) {
            return dialogue.LocalDatabase.Strings;
        }
    }
}
