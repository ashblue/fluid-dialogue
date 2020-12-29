using CleverCrow.Fluid.Databases;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    [CreateMenu("Database/Globals/Set String")]
    public class SetGlobalString : SetLocalVariableBase<string> {
        [SerializeField]
        public KeyValueDefinitionString _variable;

        protected override KeyValueDefinitionBase<string> Variable => _variable;

        protected override IKeyValueData<string> GetDatabase (IDialogueController dialogue) {
            return GlobalDatabaseManager.Instance.Database.Strings;
        }
    }
}
