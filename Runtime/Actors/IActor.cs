using UnityEngine;

namespace CleverCrow.Fluid.Dialogues {
    public interface IActor {
        string DisplayName { get; }
        Sprite Portrait { get; }
    }
}
