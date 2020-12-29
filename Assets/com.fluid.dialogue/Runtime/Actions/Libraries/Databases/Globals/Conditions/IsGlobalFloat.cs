using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.Dialogues.Actions.Databases;

namespace CleverCrow.Fluid.Dialogues.Conditions.Databases {
    [CreateMenu("Database/Globals/Is Float")]
    public class IsGlobalFloat : IsFloatBase {
        protected override IKeyValueData<float> GetFloatInstance (IDialogueController dialogue) {
            return GlobalDatabaseManager.Instance.Database.Floats;
        }
    }
}
