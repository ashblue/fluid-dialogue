using CleverCrow.Fluid.Dialogues.Actions;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Examples {
    [CreateMenu("Example/Action")]
    public class ExampleAction : ActionDataBase {
        [SerializeField]
        private string _text = null;

        public override void OnStart () {
            Debug.Log(_text);
        }
    }
}
