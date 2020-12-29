using CleverCrow.Fluid.Databases;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    [CreateMenu("Database/Globals/Set Bool")]
    public class SetGlobalBool : SetLocalVariableBase<bool> {
        [SerializeField]
        public KeyValueDefinitionBool _variable;

        protected override KeyValueDefinitionBase<bool> Variable => _variable;

        protected override IKeyValueData<bool> GetDatabase (IDialogueController dialogue) {
            return GlobalDatabaseManager.Instance.Database.Bools;
        }
    }
}
