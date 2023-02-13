/*
This script loads the upgrades levels of a car. The upgrades levels of the car
 are stored in the PlayerPrefs and are loaded in this script.
*/
using UnityEngine;
using System.Collections;

/**
 * UpgradeLoader.
 *
 * @author	Mahmoud Harmouch
 * @global
 */
public class UpgradeLoader : MonoBehaviour
{
    [Space(4)]
    [Header("Enter Car ID")]
    // Car id to load this upgrade
    [field: SerializeField]
    int carID = 0;

    [Space(3)]
    [Header("Assign Car Controller")]
    // Get car controller
    [field: SerializeField]
    CarController carController;

    [Space(3)]
    [Header("Upgrade values")]
    // How much upgrade car for each level
    [field: SerializeField]
    float[] engineUpgrade,
        speedUpgrade,
        rotateUpgrade,
        fuelUpgrade;

    [field: SerializeField]
    GameManager manager;

    void Start() { }

    void Awake()
    {
        // Read from upgrade menu
        carController.motorPower = engineUpgrade[PlayerPrefs.GetInt("Engine" + carID.ToString())];
        carController.maxSpeed = speedUpgrade[PlayerPrefs.GetInt("Speed" + carID.ToString())];

        // Suspension upgrade used as car rotate force on air (when isgrounded is false in CarController script)
        carController.RotateForce = rotateUpgrade[
            PlayerPrefs.GetInt("Suspension" + carID.ToString())
        ];

        manager = GameObject.FindObjectOfType<GameManager>();

        manager.FuelTime = fuelUpgrade[PlayerPrefs.GetInt("Fuel" + carID.ToString())];
    }
}
