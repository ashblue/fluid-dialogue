using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CleverCrow.Fluid.Dialogues.Examples {
    public class ChoiceButton : MonoBehaviour {
        public Text title;
        public Button button;
        public UnityEvent<int> clickEvent = new ActivateChoiceIndexEvent();

        private class ActivateChoiceIndexEvent : UnityEvent<int> {
        }

        private void Awake () {
            button.onClick.AddListener(() => {
                clickEvent.Invoke(transform.GetSiblingIndex());
            });
        }
    }
}
