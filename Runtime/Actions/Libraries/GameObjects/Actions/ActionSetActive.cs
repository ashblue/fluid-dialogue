using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.GameObjects {
    [CreateMenu("GameObject/Set Active")]
    public class ActionSetActive : ActionDataBase {
        [SerializeField]
        private string _gameObjectName;

        [SerializeField]
        private bool _setActive;

        public override void OnStart () {
            var target = GameObjectUtilities.FindGameObject(_gameObjectName);
            if (target == null) return;

            target.SetActive(_setActive);
        }
    }
}
