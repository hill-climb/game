/*
This script instantiates the player's selected car.
*/
using UnityEngine;
using System.Collections;

/**
 * StartPoint.
 *
 * @author	Mahmoud Harmouch
 * @global
 */
public class StartPoint : MonoBehaviour
{
    // A variable that will hold the car objects.
    [field: SerializeField]
    GameObject[] cars;

    /**
     * A Start method that runs when the script is started.
     *
     * @return	void
     */
    void Start()
    {
        // Instantiates the player's selected car by getting the car from the cars array using
        // the PlayerPrefs.GetInt ("SelectedCar") method, which returns the integer value of
        // the player's selected car, as the index of the car in the cars array. The Instantiate
        // method creates a new instance of the car object, sets its position to the transform.position
        // of the current StartPoint object, and its rotation to transform.rotation.
        Instantiate(
            cars[PlayerPrefs.GetInt("SelectedCar")],
            transform.position,
            transform.rotation
        );
    }
}
