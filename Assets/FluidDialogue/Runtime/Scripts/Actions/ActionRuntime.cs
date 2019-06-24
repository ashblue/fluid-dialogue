using System;

namespace CleverCrow.Fluid.Dialogues.Actions {
    public class ActionRuntime : IAction {
        private bool _initUsed;
        private bool _startUsed;
        private bool _resetReady;
        private bool _active;

        public string UniqueId { get; }

        public Action OnInit { private get; set; }
        public Action OnStart { private get; set; }
        public Func<ActionStatus> OnUpdate { private get; set; }
        public Action OnExit { private get; set; }
        public Action OnReset { private get; set; }

        public ActionRuntime (string uniqueId) {
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

            OnInit?.Invoke();
        }

        private void Start () {
            if (_startUsed) return;
            _startUsed = true;
            _active = true;

            OnStart?.Invoke();
        }

        private void Exit () {
            _startUsed = false;
            _resetReady = true;
            _active = false;

            OnExit?.Invoke();
        }

        private ActionStatus Update () {
            return OnUpdate();
        }

        private void Reset () {
            if (!_resetReady) return;
            _resetReady = false;
            OnReset?.Invoke();
        }
    }
}
