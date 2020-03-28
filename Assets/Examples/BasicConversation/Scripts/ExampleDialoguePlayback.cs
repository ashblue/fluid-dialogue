using System.Collections;
using System.Linq;
using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.Dialogues.Graphs;
using UnityEngine;
using UnityEngine.UI;

namespace CleverCrow.Fluid.Dialogues.Examples {
    public class ExampleDialoguePlayback : MonoBehaviour {
        private DialogueController _ctrl;

        public DialogueGraph dialogue;

        public GameObjectOverride[] gameObjectOverrides;

        [Header("Graphics")]
        public GameObject speakerContainer;
        public Image portrait;
        public Text lines;

        public RectTransform choiceList;
        public ChoiceButton choicePrefab;

        private void Awake () {
            var database = new DatabaseInstanceExtended();
           _ctrl = new DialogueController(database);

           _ctrl.Events.Speak.AddListener((actor, text) => {
               ClearChoices();
               portrait.sprite = actor.Portrait;
               lines.text = text;

               StartCoroutine(NextDialogue());
           });

           _ctrl.Events.Choice.AddListener((actor, text, choices) => {
               ClearChoices();
               portrait.sprite = actor.Portrait;
               lines.text = text;

               choices.ForEach(c => {
                   var choice = Instantiate(choicePrefab, choiceList);
                   choice.title.text = c.Text;
                   choice.clickEvent.AddListener(_ctrl.SelectChoice);
               });
           });

           _ctrl.Events.End.AddListener(() => {
               speakerContainer.SetActive(false);
           });

           _ctrl.Events.NodeEnter.AddListener((node) => {
               Debug.Log($"Node Enter: {node.GetType()} - {node.UniqueId}");
           });

           _ctrl.Play(dialogue, gameObjectOverrides.ToArray<IGameObjectOverride>());
        }

        private void ClearChoices () {
            foreach (Transform child in choiceList) {
                Destroy(child.gameObject);
            }
        }

        private IEnumerator NextDialogue () {
            yield return null;

            while (!Input.GetMouseButtonDown(0)) {
                yield return null;
            }

            _ctrl.Next();
        }

        private void Update () {
            // Required to run actions that may span multiple frames
            _ctrl.Tick();
        }
    }
}
