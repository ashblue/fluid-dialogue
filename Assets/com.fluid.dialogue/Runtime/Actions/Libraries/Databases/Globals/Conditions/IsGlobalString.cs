using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.Dialogues.Actions.Databases;

namespace CleverCrow.Fluid.Dialogues.Conditions.Databases {
    [CreateMenu("Database/Globals/Is String")]
    public class IsGlobalString : IsStringBase {
        protected override IKeyValueData<string> GetStringInstance (IDialogueController dialogue) {
            return GlobalDatabaseManager.Instance.Database.Strings;
        }
    }
}
