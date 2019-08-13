using System.Collections;
using CleverCrow.Fluid.Databases;
using CleverCrow.Fluid.Dialogues.Graphs;
using UnityEngine;
using UnityEngine.UI;

namespace CleverCrow.Fluid.Dialogues.Examples {
    public class ExampleDialoguePlayback : MonoBehaviour {
        private DialogueController _ctrl;

        public DialogueGraph dialogue;

        [Header("Graphics")]
        public GameObject speakerContainer;
        public Image portrait;
        public Text lines;

        private void Awake () {
            var database = new DatabaseInstance();
           _ctrl = new DialogueController(database);

           _ctrl.Events.Speak.AddListener((actor, text) => {
               portrait.sprite = actor.Portrait;
               lines.text = text;

               StartCoroutine(NextDialogue());
           });

           _ctrl.Events.Choice.AddListener((actor, text, choices) => {
               portrait.sprite = actor.Portrait;
               lines.text = text;

               // @TODO Add choice selection logic
           });

           _ctrl.Events.End.AddListener(() => {
               speakerContainer.SetActive(false);
           });

           _ctrl.Play(dialogue);
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
