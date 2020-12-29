using CleverCrow.Fluid.Databases;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    [CreateMenu("Database/Locals/Is Int")]
    public class IsLocalInt : IsIntBase {
        protected override IKeyValueData<int> GetIntInstance (IDialogueController dialogue) {
            return dialogue.LocalDatabase.Ints;
        }
    }
}
