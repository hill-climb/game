/*
This script defines the behavior of a game object that acts as a finish trigger.
*/
using UnityEngine;
using System.Collections;

/**
 * FinishTrigger class.
 *
 * @author	Mahmoud Harmouch
 * @global
 */
public class FinishTrigger : MonoBehaviour
{
    // A reference to a game object that will represent the finish menu in the game.
    [field: SerializeField]
    GameObject finishMenu;

    /**
     * An optional method and it is left empty.
     */
    void Start() { }

    /**
     * This function is called once per frame and is used for updating the object's state.
     * Like the "Start" function, it is also left empty in this script.
     */
    void Update() { }

    /**
     * A special function that is called when a collider enters the trigger.
     *
     * @param	collider	col - A reference to the collider that entered the trigger.
     * @return	void
     */
    void OnTriggerEnter(Collider col)
    {
        // Check if the collider is tagged as "Player", then set the "finishMenu" object
        // to be active, making it visible in the game.
        if (col.CompareTag("Player"))
        {
            finishMenu.SetActive(true);
        }
    }
}
