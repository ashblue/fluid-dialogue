using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Editors.Inspectors {
    public class TypesToMenu<T> {
        public class TypeEntry {
            public Type type;
            public string path;
            public int priority;
        }

        public List<TypeEntry> Lines { get; }

        public TypesToMenu () {
            Lines = GetTypeEntries();
        }

        private static List<TypeEntry> GetTypeEntries () {
            var list = new List<TypeEntry>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                foreach (var type in assembly.GetTypes()) {
                    if (!type.IsSubclassOf(typeof(T)) || type.IsAbstract) continue;
                    var attr = type.GetCustomAttribute<CreateMenuAttribute>();

                    list.Add(new TypeEntry {
                        type = type,
                        path = attr?.Path ?? type.FullName,
                        priority = attr?.Priority ?? 0,
                    });
                }
            }

            return list
                .OrderByDescending(t => t.priority)
                .ToList();
        }
    }
}
