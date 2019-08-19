namespace CleverCrow.Fluid.Dialogues.Conditions {
    public interface ICondition : IUniqueId {
        bool GetIsValid ();
    }
}
