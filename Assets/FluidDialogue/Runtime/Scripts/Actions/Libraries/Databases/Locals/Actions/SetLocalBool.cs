using CleverCrow.Fluid.Databases;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    public class SetLocalBool : SetLocalVariableBase<bool> {
        [SerializeField]
        public KeyValueDefinitionBool _definition;

        protected override KeyValueDefinitionBase<bool> Definition => _definition;

        protected override IKeyValueData<bool> GetDatabase (IDialogueController dialogue) {
            return dialogue.LocalDatabase.Bools;
        }
    }
}
