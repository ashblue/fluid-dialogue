namespace CleverCrow.Fluid.Dialogues.Actions {
    public abstract class ActionBase : IAction {
        private bool _initUsed;
        private bool _startUsed;

        public ActionStatus Tick () {
            Init();
            Start();

            return ActionStatus.Success;
        }

        public void End () {
            throw new System.NotImplementedException();
        }

        public void Init () {
            if (_initUsed) return;
            _initUsed = true;

            OnInit();
        }
        protected virtual void OnInit () {}

        public void Start () {
            if (_startUsed) return;
            _startUsed = true;

            OnStart();
        }
        protected virtual void OnStart () {}

        public void Exit () {
            _startUsed = false;
        }
    }
}
