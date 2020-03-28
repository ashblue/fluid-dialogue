using CleverCrow.Fluid.Dialogues.GameObjectVariables;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.GameObjects {
    [CreateMenu("GameObject/Set Active")]
    public class SetActive : ActionDataBase {
        private IDialogueController _dialogue;

        [SerializeField]
        private KeyValueDefinitionGameObject _gameObject = null;

        [SerializeField]
        private bool _setActive = false;

        public override void OnInit (IDialogueController dialogue) {
            _dialogue = dialogue;
        }

        public override void OnStart () {
            var go = _dialogue.LocalDatabaseExtended.GameObjects.Get(_gameObject.key, _gameObject.defaultValue);
            var goWrapper = new GameObjectWrapper(go);

            var setActive = new SetActiveInternal(goWrapper);
            setActive.SetValue(_setActive);
        }
    }

    public class SetActiveInternal {
        private readonly IGameObjectWrapper _go;

        public SetActiveInternal (IGameObjectWrapper go) {
            _go = go;
        }

        public void SetValue (bool value) {
            _go.SetActive(value);
        }
    }
}
