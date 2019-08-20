namespace CleverCrow.Fluid.Dialogues.Conditions {
    public interface IConditionData {
        void OnInit (IDialogueController dialogue);
        bool OnGetIsValid ();
    }

    public class ConditionRuntime : ICondition {
        private readonly IDialogueController _dialogueController;
        private readonly IConditionData _data;

        private bool _initTriggered;

        public string UniqueId { get; }

        public ConditionRuntime (IDialogueController dialogueController, string uniqueId, IConditionData data) {
            _data = data;
            _dialogueController = dialogueController;
            UniqueId = uniqueId;
        }

        public bool GetIsValid () {
            if (!_initTriggered) {
                _data.OnInit(_dialogueController);
                _initTriggered = true;
            }

            return _data.OnGetIsValid();
        }
    }
}
