using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.GameObjects {
    [CreateMenu("GameObject/Send Message/Int")]
    public class ActionSendMessageInt : ActionDataBase {
        [SerializeField]
        private string _gameObjectName;

        [SerializeField]
        private string _methodName;

        [SerializeField]
        private int _value;

        [SerializeField]
        SendMessageOptions _options;

        public override void OnStart () {
            var target = GameObjectUtilities.FindGameObject(_gameObjectName);
            if (target == null) return;

            target.SendMessage(_methodName, _value, _options);
        }
    }
}
