using CleverCrow.Fluid.Databases;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    public class SetLocalString : SetLocalVariableBase<string> {
        [SerializeField]
        public KeyValueDefinitionString _definition;

        protected override KeyValueDefinitionBase<string> Definition => _definition;

        protected override IKeyValueData<string> GetDatabase (IDialogueController dialogue) {
            return dialogue.LocalDatabase.Strings;
        }
    }
}
