using System;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.Actions.GameObjects {
    public static class GameObjectUtilities {
        public static GameObject FindGameObject (string name) {
            var target = GameObject.Find(name);

            if (target == null) {
                // Try to find inactive object's parent one level up
                // @NOTE Only works if the parent is active for runtime performance reasons
                var hasParent = name.Contains("/");
                if (hasParent) {
                    var parentPath = name.Substring(0, name.LastIndexOf("/", StringComparison.Ordinal));
                    var parent = GameObject.Find(parentPath);

                    // We know we have a parent, now we need to find the inactive child
                    if (parent != null) {
                        var objectName = name.Substring(name.LastIndexOf("/", StringComparison.Ordinal) + 1);
                        foreach (Transform child in parent.transform) {
                            if (child.gameObject.name == objectName) {
                                target = child.gameObject;
                                break;
                            }
                        }
                    }
                } else {
                    // Look at top level inactive objects only for performance reasons
                    // @NOTE This only works if the object is in the active scene
                    var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                    var rootObjects = currentScene.GetRootGameObjects();
                    foreach (var obj in rootObjects) {
                        if (obj.name == name) {
                            target = obj;
                            break;
                        }
                    }
                }
            }

            // Give up and fire an error
            if (target == null) {
                Debug.LogError($"GameObject not found: {name}. SendMessage action will not run. Make sure the object path is correct.\nIf the object is inactive it must be nested under an active object or in the active scene if using async loading.");
                return null;
            }

            return target;
        }
    }
}
