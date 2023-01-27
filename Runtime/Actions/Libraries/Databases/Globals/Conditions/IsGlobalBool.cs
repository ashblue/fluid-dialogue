using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.Dialogues.Actions.Databases;

namespace CleverCrow.Fluid.Dialogues.Conditions.Databases {
    [CreateMenu("Database/Globals/Is Bool")]
    public class IsGlobalBool : IsBoolBase {
        protected override IKeyValueData<bool> GetBoolInstance (IDialogueController dialogue) {
            return GlobalDatabaseManager.Instance.Database.Bools;
        }
    }
}
