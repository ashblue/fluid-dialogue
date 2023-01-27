using CleverCrow.Fluid.Databases;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    [CreateMenu("Database/Locals/Set Float")]
    public class SetLocalFloat : SetLocalVariableBase<float> {
        [SerializeField]
        public KeyValueDefinitionFloat _variable;

        protected override KeyValueDefinitionBase<float> Variable => _variable;

        protected override IKeyValueData<float> GetDatabase (IDialogueController dialogue) {
            return dialogue.LocalDatabase.Floats;
        }
    }
}
