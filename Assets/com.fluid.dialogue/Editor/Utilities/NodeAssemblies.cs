using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CleverCrow.Fluid.Dialogues.Editors.NodeDisplays;
using CleverCrow.Fluid.Dialogues.Nodes;

namespace CleverCrow.Fluid.Dialogues.Editors {
    public static class NodeAssemblies {
        private static Dictionary<Type, Type> _dataToNode;
        private static Dictionary<string, Type> _stringToData;

        public static Dictionary<Type, Type> DataToDisplay =>
            _dataToNode ?? (_dataToNode = GetDataToDisplay());

        public static Dictionary<string, Type> StringToData =>
            _stringToData ?? (_stringToData = GetStringToData());

        private static Dictionary<Type, Type> GetDataToDisplay () {
            var displayTypes = Assembly
                .GetAssembly(typeof(NodeEditorBase))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(NodeEditorBase)));

            return displayTypes.ToDictionary(
                (k) => {
                    var attribute = k.GetCustomAttribute<NodeTypeAttribute>();
                    return attribute.Type;
                },
                (v) => v);
        }

        private static Dictionary<string, Type> GetStringToData () {
            var menuTypes = Assembly
                .GetAssembly(typeof(NodeDataBase))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(NodeDataBase))
                            && t.GetCustomAttribute<CreateMenuAttribute>() != null)
                .OrderByDescending(t => t.GetCustomAttribute<CreateMenuAttribute>().Priority);

            return menuTypes.ToDictionary(
                (k) => {
                    var attribute = k.GetCustomAttribute<CreateMenuAttribute>();
                    return attribute.Path;
                },
                (v) => v);
        }
    }
}
