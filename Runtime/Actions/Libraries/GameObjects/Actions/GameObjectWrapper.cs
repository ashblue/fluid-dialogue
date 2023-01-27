using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.GameObjects {
    public interface IGameObjectWrapper {
        void SetActive (bool value);
    }

    public class GameObjectWrapper : IGameObjectWrapper {
        private readonly GameObject _go;

        public GameObjectWrapper (GameObject go) {
            _go = go;
        }

        public void SetActive (bool value) {
            _go.SetActive(value);
        }
    }
}
