namespace CleverCrow.Fluid.Dialogues.Actions {
    public interface IAction {
        ActionStatus Tick ();
        void End ();
    }
}
