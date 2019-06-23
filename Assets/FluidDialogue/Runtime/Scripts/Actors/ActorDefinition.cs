using UnityEngine;

namespace CleverCrow.Fluid.Dialogues {
    public class ActorDefinition : ScriptableObject, IActor {
        [SerializeField]
        private string _name = null;

        public string Name => _name;
    }
}
