using CleverCrow.Fluid.Databases;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    [CreateMenu("Database/Globals/Set Float")]
    public class SetGlobalFloat : SetLocalVariableBase<float> {
        [SerializeField]
        public KeyValueDefinitionFloat _variable;

        protected override KeyValueDefinitionBase<float> Variable => _variable;

        protected override IKeyValueData<float> GetDatabase (IDialogueController dialogue) {
            return GlobalDatabaseManager.Instance.Database.Floats;
        }
    }
}
