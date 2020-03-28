using CleverCrow.Fluid.Databases;
using UnityEngine;

namespace CleverCrow.Fluid.Dialogues.GameObjectVariables {
    [CreateAssetMenu(
        menuName = CREATE_PATH + "/Key Value GameObject",
        fileName = "KeyValueGameObject"
    )]
    public class KeyValueDefinitionGameObject : KeyValueDefinitionBase<GameObject> {
    }
}
