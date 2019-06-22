using UnityEngine.Events;

namespace CleverCrow.Fluid.Dialogues {
    public class UnityEventPlus : UnityEvent, IUnityEvent {
    }

    public class UnityEventPlus<T1, T2> : UnityEvent<T1, T2>, IUnityEvent<T1, T2> {
    }

    public class UnityEventPlus<T1, T2, T3> : UnityEvent<T1, T2, T3>, IUnityEvent<T1, T2, T3> {
    }
}
