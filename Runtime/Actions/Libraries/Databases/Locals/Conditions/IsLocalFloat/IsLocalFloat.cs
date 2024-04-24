using CleverCrow.Fluid.Databases;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    [CreateMenu("Database/Locals/Is Float")]
    public class ConditionLocalFloat : IsFloatBase {
        protected override IKeyValueData<float> GetFloatInstance (IDialogueController dialogue) {
            return dialogue.LocalDatabase.Floats;
        }
    }
}
