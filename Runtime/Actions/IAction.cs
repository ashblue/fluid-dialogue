namespace CleverCrow.Fluid.Dialogues.Actions {
    public interface IAction : IUniqueId {
        ActionStatus Tick ();
        void End ();
    }
}
