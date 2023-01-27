using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.Dialogues.Actions.Databases;

namespace CleverCrow.Fluid.Dialogues.Conditions.Databases {
    [CreateMenu("Database/Globals/Is Int")]
    public class IsGlobalInt : IsIntBase {
        protected override IKeyValueData<int> GetIntInstance (IDialogueController dialogue) {
            return GlobalDatabaseManager.Instance.Database.Ints;
        }
    }
}
