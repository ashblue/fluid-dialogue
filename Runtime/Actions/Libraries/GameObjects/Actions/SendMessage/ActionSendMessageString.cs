using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.GameObjects {
    [CreateMenu("GameObject/Send Message/String")]
    public class ActionSendMessageString : ActionDataBase {
        [SerializeField]
        private string _gameObjectName;

        [SerializeField]
        private string _methodName;

        [SerializeField]
        private string _value;

        [SerializeField]
        SendMessageOptions _options;

        public override void OnStart () {
            var target = GameObjectUtilities.FindGameObject(_gameObjectName);
            if (target == null) return;

            target.SendMessage(_methodName, _value, _options);
        }
    }
}
