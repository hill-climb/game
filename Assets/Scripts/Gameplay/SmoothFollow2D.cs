/*
This script defines the camera follow mechanism.
*/
using UnityEngine;
using System.Collections;

/**
 * SmoothFollow2D.
 *
 * @author	Mahmoud Harmouch
 * @global
 */
public class SmoothFollow2D : MonoBehaviour
{
    // Set the velocity of the camera to a predefined constant representing a zero vector in 3D space.
    [field: SerializeField]
    Vector3 velocity = Vector3.zero;

    // A variable used to store the Transform component of the component tageed "Player" to be followed
    // by the camera.
    [field: SerializeField]
    Transform target;

    [field: SerializeField]
    string targetTag = "Player";

    // A relative position of the target object within the camera's viewport.
    [field: SerializeField]
    Vector2 position = new Vector2(0.3f, 0.5f);

    /**
     * Start.
     *
     * @return	mixed
     */
    IEnumerator Start()
    {
        // wait until the end of the current frame before executing the next line.
        yield return new WaitForEndOfFrame();
        target = GameObject.FindGameObjectWithTag(targetTag).transform;
    }

    /**
     * Update.
     *
     * @return	void
     */
    void Update()
    {
        // Check if the target variable is set.
        if (target)
        {
            // Calculate a viewport point of the target's position relative to the camera.
            Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
            // Calculate the difference between the target's world position and the position
            // of the same point in the viewport space.
            Vector3 delta =
                target.position
                - GetComponent<Camera>()
                    .ViewportToWorldPoint(new Vector3(position.x, position.y, point.z));

            // Add this difference to the current position of the transform component attached
            // to this MonoBehaviour's GameObject to calculate a destination vector.
            Vector3 destination = transform.position + delta;
            // Set the transform's position to the destination vector, with a smooth damping effect.
            // The ref velocity parameter passed to SmoothDamp is a reference to the velocity class
            // variable that is used as an internal velocity for the smooth damping effect.
            // The last parameter 0 sets the smooth damping time to 0, so the position is
            // instantly set to the destination vector.
            transform.position = Vector3.SmoothDamp(
                transform.position,
                destination,
                ref velocity,
                0
            );
        }
    }
}
