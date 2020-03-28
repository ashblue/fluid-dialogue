using CleverCrow.Fluid.Databases;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues {
    public interface IGameObjectOverride {
        IKeyValueDefinition<GameObject> Definition { get; }
        GameObject Value { get; }
    }
}
