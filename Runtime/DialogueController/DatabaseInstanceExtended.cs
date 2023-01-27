using CleverCrow.Fluid.Databases;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues {
    public class DatabaseInstanceExtended : DatabaseInstance, IDatabaseInstanceExtended {
        public IKeyValueData<GameObject> GameObjects { get; } = new KeyValueDataGameObject();

        public void ClearGameObjects () {
            GameObjects.Clear();
        }
    }
}
