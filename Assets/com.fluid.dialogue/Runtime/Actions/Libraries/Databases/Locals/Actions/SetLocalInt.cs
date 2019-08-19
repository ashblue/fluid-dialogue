using CleverCrow.Fluid.Databases;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    [CreateMenu("Database/Locals/Set Int")]
    public class SetLocalInt : SetLocalVariableBase<int> {
        [SerializeField]
        public KeyValueDefinitionInt _variable;

        protected override KeyValueDefinitionBase<int> Variable => _variable;

        protected override IKeyValueData<int> GetDatabase (IDialogueController dialogue) {
            return dialogue.LocalDatabase.Ints;
        }
    }
}
