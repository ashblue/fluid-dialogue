using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            return Assembly
                .GetAssembly(typeof(T))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(T)) && !t.IsAbstract)
                .Select(t => {
                    var attr = t.GetCustomAttribute<CreateMenuAttribute>();

                    return new TypeEntry {
                        type = t,
                        path = attr?.Path ?? t.FullName,
                        priority = attr?.Priority ?? 0,
                    };
                })
                .OrderByDescending(t => t.priority)
                .ToList();
        }
    }
}
