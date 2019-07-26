using System;
using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.Dialogues.Nodes;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Choices {
    public class ChoiceData : ScriptableObject, IGetRuntime<IChoice> {
        public string text;
        public List<NodeDataBase> children;

        [SerializeField]
        private string _uniqueId;

        public string UniqueId => _uniqueId;

        public void Setup () {
            _uniqueId = Guid.NewGuid().ToString();
        }

        public IChoice GetRuntime (IDialogueController dialogue) {
            return new ChoiceRuntime(
                _uniqueId,
                children.Select(c => c.GetRuntime(dialogue)).ToList());
        }
    }
}
