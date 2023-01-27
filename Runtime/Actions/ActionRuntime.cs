namespace CleverCrow.Fluid.Dialogues.Actions {
    public interface IActionData {
        void OnInit (IDialogueController dialogue);
        void OnStart ();
        ActionStatus OnUpdate ();
        void OnExit ();
        void OnReset ();
    }

    public class ActionRuntime : IAction {
        private readonly IDialogueController _dialogueController;
        private readonly IActionData _data;

        private bool _initUsed;
        private bool _startUsed;
        private bool _resetReady;
        private bool _active;

        public string UniqueId { get; }

        public ActionRuntime (IDialogueController dialogue, string uniqueId, IActionData data) {
            _data = data;
            _dialogueController = dialogue;
            UniqueId = uniqueId;
        }

        public ActionStatus Tick () {
            Reset();
            Init();
            Start();

            var status = Update();
            if (status == ActionStatus.Success) {
                Exit();
            }

            return status;
        }

        public void End () {
            if (_active) Exit();
        }

        private void Init () {
            if (_initUsed) return;
            _initUsed = true;

            _data.OnInit(_dialogueController);
        }

        private void Start () {
            if (_startUsed) return;
            _startUsed = true;
            _active = true;

            _data.OnStart();
        }

        private void Exit () {
            _startUsed = false;
            _resetReady = true;
            _active = false;

            _data.OnExit();
        }

        private ActionStatus Update () {
            return _data.OnUpdate();
        }

        private void Reset () {
            if (!_resetReady) return;
            _resetReady = false;
            _data.OnReset();
        }
    }
}
