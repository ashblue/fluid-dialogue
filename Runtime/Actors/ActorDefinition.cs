using UnityEngine;

namespace CleverCrow.Fluid.Dialogues {
    [CreateAssetMenu(fileName = "Actor", menuName = "Fluid/Dialogue/Actor")]
    public class ActorDefinition : ScriptableObject, IActor {
        [SerializeField]
        private string _displayName = null;

        [SerializeField]
        private Sprite _portrait = null;

        public string DisplayName => _displayName;
        public Sprite Portrait => _portrait;
    }
}
