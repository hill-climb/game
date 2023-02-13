/*
The script is used to spawn items in the scene.
*/
using UnityEngine;
using System.Collections;

/**
 * The ItemSpawner class.
 *
 * @author	Mahmoud Harmouch
 * @global
 */
public class ItemSpawner : MonoBehaviour
{
    // The items that are to be spawned.
    [field: SerializeField]
    GameObject[] Items;

    // A reference to the parent GameObject which the items will be a child of.
    [field: SerializeField]
    GameObject parent;

    // A reference to the GameObject that is responsible for moving the parent
    // GameObject along with its children items.
    [field: SerializeField]
    GameObject mover;
}
