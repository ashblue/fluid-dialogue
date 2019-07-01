using CleverCrow.Fluid.Databases;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    public class SetLocalFloat : SetLocalVariableBase<float> {
        [SerializeField]
        public KeyValueDefinitionFloat _definition;

        protected override KeyValueDefinitionBase<float> Definition => _definition;

        protected override IKeyValueData<float> GetDatabase (IDialogueController dialogue) {
            return dialogue.LocalDatabase.Floats;
        }
    }
}
