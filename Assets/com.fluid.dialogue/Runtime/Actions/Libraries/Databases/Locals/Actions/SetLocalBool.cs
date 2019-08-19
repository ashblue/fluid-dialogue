using CleverCrow.Fluid.Databases;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    [CreateMenu("Database/Locals/Set Bool")]
    public class SetLocalBool : SetLocalVariableBase<bool> {
        [SerializeField]
        public KeyValueDefinitionBool _variable;

        protected override KeyValueDefinitionBase<bool> Variable => _variable;

        protected override IKeyValueData<bool> GetDatabase (IDialogueController dialogue) {
            return dialogue.LocalDatabase.Bools;
        }
    }
}
