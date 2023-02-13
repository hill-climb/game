/*
This script defines the needles rotation mechanism.
*/
using UnityEngine;
using System.Collections;

/**
 * MeterNeedle.
 *
 * @author	Mahmoud Harmouch
 * @global
 */
public class MeterNeedle : MonoBehaviour
{
    [HideInInspector]
    [field: SerializeField]
    CarController carController;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(.3f);
        carController = GameObject.FindObjectOfType<CarController>();
    }

    [SerializeField]
    [Range(0f, 1f)]
    float multiplier = 0.5f;

    [SerializeField]
    float minRotation = 135f;

    [SerializeField]
    float maxRotation = -135f;

    /**
     * Update.
     *
     * @return	void
     */
    void Update()
    { // 2600 is the max motor speed.
        if (carController)
        {
            RotateNeedle(Mathf.Abs(carController.GetInput() / 2600));
        }
    }

    public void RotateNeedle(float percent)
    {
        // `percent` is clamped between 0 and 1 to ensure it stays within the desired range.
        percent = percent > 1f ? 1f : percent;
        percent = percent < 0f ? 0f : percent;
        // The rotation calculation uses minRotation, maxRotation, and multiplier
        // to determine the final rotation of the needle. The rotation is applied
        // to the transform component of the needle using transform.eulerAngles
        // and a Vector3 value representing the rotation axis (z-axis in this case).
        transform.eulerAngles =
            Vector3.forward * (minRotation + percent * 2f * maxRotation * multiplier);
    }
}
