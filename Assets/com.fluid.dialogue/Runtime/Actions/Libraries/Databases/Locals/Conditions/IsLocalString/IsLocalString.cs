using CleverCrow.Fluid.Databases;

namespace CleverCrow.Fluid.Dialogues.Actions.Databases {
    [CreateMenu("Database/Locals/Is String")]
    public class IsLocalString : IsStringBase {
        protected override IKeyValueData<string> GetStringInstance (IDialogueController dialogue) {
            return dialogue.LocalDatabase.Strings;
        }
    }
}
