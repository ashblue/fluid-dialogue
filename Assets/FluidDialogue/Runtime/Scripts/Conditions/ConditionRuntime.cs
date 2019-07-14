using System;

namespace CleverCrow.Fluid.Dialogues.Conditions {
    public class ConditionRuntime : ICondition {
        private readonly IDialogueController _dialogueController;

        private bool _initTriggered;

        public string UniqueId { get; }

        public Func<bool> OnGetIsValid { private get; set; }
        public Action<IDialogueController> OnInit { private get; set; }

        public ConditionRuntime (IDialogueController dialogueController, string uniqueId) {
            _dialogueController = dialogueController;
            UniqueId = uniqueId;
        }

        public bool GetIsValid () {
            if (!_initTriggered) {
                OnInit?.Invoke(_dialogueController);
                _initTriggered = true;
            }

            return OnGetIsValid?.Invoke() ?? true;
        }
    }
}
