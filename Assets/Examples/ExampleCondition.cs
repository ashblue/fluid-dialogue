using CleverCrow.Fluid.Dialogues.Conditions;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Examples {
    [CreateMenu("Example/Condition")]
    public class ExampleCondition : ConditionDataBase {
        [SerializeField]
        private bool _isValid = false;

        public override bool OnGetIsValid (INode parent) {
            Debug.Log($"Example Condition: Returned {_isValid} for node {parent.UniqueId}");
            return _isValid;
        }
    }
}
