using UnityEngine.Events;

namespace CleverCrow.Fluid.Dialogues {
    public interface IUnityEvent {
        void Invoke ();
        void AddListener (UnityAction action);
    }

    public interface IUnityEvent<T1, T2> {
        void Invoke (T1 t1, T2 t2);
        void AddListener (UnityAction<T1, T2> action);
    }

    public interface IUnityEvent<T1, T2, T3> {
        void Invoke (T1 t1, T2 t2, T3 t3);
        void AddListener (UnityAction<T1, T2, T3> action);
    }
}
