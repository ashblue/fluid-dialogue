using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CleverCrow.Fluid.Dialogues.Editors.NodeDisplays;
using CleverCrow.Fluid.Dialogues.Graphs;
using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public class DialogueWindow : EditorWindow {
        private DialogueGraph _graph;
        private Dictionary<Type, Type> _nodeDisplays;
        private List<NodeDisplayBase> _nodes = new List<NodeDisplayBase>();

        public static void ShowGraph (DialogueGraph graph) {
            var window = GetWindow<DialogueWindow>(false);
            window.SetGraph(graph);
        }

        private void SetGraph (DialogueGraph graph) {
            if (_nodeDisplays == null) {
                _nodeDisplays = GetNodeDisplays();
            }

            _nodes = graph.Nodes
                .Select((n) => {
                    var displayType = _nodeDisplays[n.GetType()];
                    var instance = Activator.CreateInstance(displayType) as NodeDisplayBase;
                    if (instance == null) throw new NullReferenceException($"No type found for ${n}");
                    instance.Setup(n);
                    return instance;
                })
                .ToList();

            _graph = graph;
        }

        private static Dictionary<Type, Type> GetNodeDisplays () {
            var displayTypes = Assembly
                .GetAssembly(typeof(NodeDisplayBase))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(NodeDisplayBase)));

            return displayTypes.ToDictionary(
                (k) => {
                    var attribute = k.GetCustomAttribute<NodeTypeAttribute>();
                    return attribute.Type;
                },
                (v) => v);
        }

        private void OnGUI () {
            if (_graph == null) return;

            GUI.Label(new Rect(10, 10, 300, 100), $"Dialogue: {_graph.name}", EditorStyles.boldLabel);

            foreach (var node in _nodes) {
                node.Print();
            }
        }
    }
}
