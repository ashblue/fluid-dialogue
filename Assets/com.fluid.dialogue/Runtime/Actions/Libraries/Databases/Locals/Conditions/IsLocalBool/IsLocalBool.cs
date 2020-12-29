using CleverCrow.Fluid.Databases;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    [CreateMenu("Database/Locals/Is Bool")]
    public class IsLocalBool : IsBoolBase {
        protected override IKeyValueData<bool> GetBoolInstance (IDialogueController dialogue) {
            return dialogue.LocalDatabase.Bools;
        }
    }
}
