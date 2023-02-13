/*
The script is used to determine the current selected item in a selection panel,
e.g. a car selection panel or level selection panel.
*/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/**
 * Define a new class "CurrentSelected" that inherits from the MonoBehaviour class.
 *
 * @author	Mahmoud Harmouch
 * @global
 */
public class CurrentSelected : MonoBehaviour
{
    // Declare a variable that represents the ID of the button in the selection panel
    // and create a header labeled "Button ID" in the Inspector.
    [Header("Button ID")]
    [field: SerializeField]
    int id;

    // Declare a variable that stores a reference to the center point of the selection
    // panel and create a header labeled "Center point" in the Inspector.
    [Header("Center point")]
    [field: SerializeField]
    Transform centerPoint;

    // Declare a variable that is used to store the distance between the button and the
    // center point.
    [field: SerializeField]
    float Dist;

    // Declare a variable that stores a reference to a "carSelect" component in the scene
    // panel and create a header labeled "Car and Level Select Components" in the Inspector.
    [Header("Car and Level Select Components")]
    [field: SerializeField]
    CarSelect carSelect;

    // Declare a variable that stores a reference to a "LevelSelect" component in the scene.
    [field: SerializeField]
    LevelSelect levelSelect;

    /**
     * This line defines the "Update" method of the script, which is called once per frame by Unity.
     *
     * @return	void
     */
    void Update()
    {
        // Calculate the distance between the button and the center point using the Vector3.Distance method.
        Dist = Vector3.Distance(transform.position, centerPoint.position);

        // Check if the distance calculated is less than 100 units.
        if (Dist < 100f)
        {
            if (carSelect)
                carSelect.id = id;

            if (levelSelect)
                levelSelect.id = id;
        }
    }
}
