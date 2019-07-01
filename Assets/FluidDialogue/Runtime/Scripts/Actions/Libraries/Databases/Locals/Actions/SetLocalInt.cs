using CleverCrow.Fluid.Databases;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    public class SetLocalInt : SetLocalVariableBase<int> {
        [SerializeField]
        public KeyValueDefinitionInt _definition;

        protected override KeyValueDefinitionBase<int> Definition => _definition;

        protected override IKeyValueData<int> GetDatabase (IDialogueController dialogue) {
            return dialogue.LocalDatabase.Ints;
        }
    }
}
