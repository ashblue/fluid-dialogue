namespace CleverCrow.Fluid.Dialogues {
    public interface IUnityEvent {
        void Invoke ();
    }

    public interface IUnityEvent<T1, T2> {
        void Invoke (T1 t1, T2 t2);
    }

    public interface IUnityEvent<T1, T2, T3> {
        void Invoke (T1 t1, T2 t2, T3 t3);
    }
}
