using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.Dialogues.GameObjectVariables;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues {
    [System.Serializable]
    public class GameObjectOverride : IGameObjectOverride {
        [SerializeField]
        private KeyValueDefinitionGameObject _variable = null;

        [SerializeField]
        private GameObject _gameObject = null;

        public IKeyValueDefinition<GameObject> Definition => _variable;
        public GameObject Value => _gameObject;
    }
}
