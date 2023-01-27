using CleverCrow.Fluid.Databases;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues {
    public interface IDatabaseInstanceExtended : IDatabaseInstance {
        IKeyValueData<GameObject> GameObjects { get; }

        void ClearGameObjects ();
    }
}
