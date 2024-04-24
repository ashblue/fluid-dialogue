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
            var dict = new Dictionary<Type, Type>();

            // Expensive to get all assemblies, but only way to get data outside the package
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                foreach (var type in assembly.GetTypes()) {
                    if (!type.IsSubclassOf(typeof(NodeEditorBase)) || type.IsAbstract) continue;

                    var attr = type.GetCustomAttribute<NodeTypeAttribute>();
                    if (attr == null) continue;

                    dict.Add(attr.Type, type);
                }
            }

            return dict;
        }

        class TypeEntry {
            public Type type;
            public string path;
            public int priority;
        }

        private static Dictionary<string, Type> GetStringToData () {
            var list = new List<TypeEntry>();

            // Expensive to get all assemblies, but only way to get data outside the package
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                foreach (var type in assembly.GetTypes()) {
                    if (!type.IsSubclassOf(typeof(NodeDataBase)) || type.IsAbstract) continue;
                    var attr = type.GetCustomAttribute<CreateMenuAttribute>();
                    if (attr == null) continue;

                    list.Add(new TypeEntry {
                        type = type,
                        path = attr?.Path ?? type.FullName,
                        priority = attr?.Priority ?? 0,
                    });
                }
            }

            return list
                .OrderByDescending(t => t.priority)
                .ToDictionary(
                    (k) => k.path,
                    (v) => v.type);
        }
    }
}
