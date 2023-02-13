/*
The script is used to control cars. It makes use of the WheelJoint2D component to drive
the car based on user input.
*/
using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour
{
    // A Transform object to store the position of the center of mass of the car's rigidbody.
    [field: SerializeField]
    Transform centerOfMass;

    // A reference for the back wheel of the car.
    [field: SerializeField]
    JointMotor2D motorBack;

    // A variable to specify which wheel to drive the car with.
    [field: SerializeField]
    WheelJoint2D motorWheel;

    // A variable to store the car's speed.
    [field: SerializeField]
    float speed;

    // Whether the car is grounded or not.
    [field: SerializeField]
    bool isGrounded;

    // How much distance from the ground means the car is considered grounded.
    [field: SerializeField]
    float groundDistance = 2.1f;

    // Variable to store the values for the power of the motor, the power of the brake,
    // and the speed of deceleration.
    public float motorPower = 1400f;

    [field: SerializeField]
    float decelerationSpeed = 0.3f;

    // The maximum speed of the car.
    public float maxSpeed = 14f;

    // A temporary variable for internal usage.
    [field: SerializeField]
    float motorTemp;

    // A variable to store whether the car can rotate or not, true when the car is in the air.
    [field: SerializeField]
    bool canRotate = false;

    // A variable to store the rotate force applied to the car when it's in the air.
    public float RotateForce = 140f;

    // An AudioSource object to play engine sound.
    [HideInInspector]
    public AudioSource EngineSoundS;

    // A variable to store whether the script is running on mobile or not.
    [field: SerializeField]
    bool isMobile;

    // A ParticleSystem object to store the wheel particle effect.
    [field: SerializeField]
    ParticleSystem wheelParticle;

    // A temporary variable for internal usage.
    [field: SerializeField]
    float powerTemp;

    // Variables for wheel particles and smoke.
    [field: SerializeField]
    ParticleSystem.EmissionModule em,
        emSmoke;

    // A Transform object to store the position of the wheel particle effect.
    [field: SerializeField]
    Transform particlePosition;

    // A bool to store whether to use smoke or not.
    [field: SerializeField]
    bool useSmoke;

    // A ParticleSystem object to store the smoke effect.
    [field: SerializeField]
    ParticleSystem smoke;

    // A float to store the target speed for the smoke effect.
    [field: SerializeField]
    float smokeTargetSpeed = 17f;

    // A float to store the distance of the camera from the car.
    [field: SerializeField]
    float cameraDistance = 15f;

    // A variable that keeps track the current speed of the car.
    [field: SerializeField]
    float currentSpeed;

    // Engine sound variables.
    [field: SerializeField]
    float Multiplyer = 3f;

    [field: SerializeField]
    float minP = 1f;

    [field: SerializeField]
    float maxP = 2.4f;

    [field: SerializeField]
    float HoriTemp;

    /**
     * This function is called once when the script is enabled. It sets the position of
     * the main camera based on the cameraDistance variable.
     *
     * @return	void
     */
    void Awake()
    {
        Vector3 posCamera;
        posCamera = Camera.main.transform.position;
        Camera.main.transform.position = new Vector3(posCamera.x, posCamera.y, -cameraDistance);
    }

    /**
     * This function is called when the script is first run.
     *
     * @return	void
     */
    void Start()
    {
        // Set the car's rigidbody center of mass to the position of the centerOfMass Transform object
        GetComponent<Rigidbody2D>().centerOfMass = centerOfMass.transform.localPosition;

        // Set up the motorBack.
        motorBack = motorWheel.motor;

        // Start the RaycCast() coroutine to check if the car is grounded.
        StartCoroutine(RaycCast());

        // Set up the engine sound.
        EngineSoundS = GetComponent<AudioSource>();

        powerTemp = motorPower;

        // Set up the wheel particle effect.
        em = wheelParticle.emission;
        em.enabled = false;

        if (smoke)
        {
            // Set up the smoke effect.
            emSmoke = smoke.emission;
            emSmoke.enabled = false;
        }
    }

    /**
     * This method is called every fixed frame rate and it updates the car's behavior.
     *
     * @return	void
     */
    void FixedUpdate()
    {
        // Limit the car's speed based on the maxSpeed value.
        if (speed > maxSpeed)
            motorPower = 0;
        else
            motorPower = powerTemp;

        // Check the horizontal input axis and move the car forward.
        if (Input.GetAxis("Horizontal") > 0 || HoriTemp > 0)
        {
            // Check if the car is grounded.
            if (isGrounded)
            {
                // Update the motorBack motor speed and rotate the car when it's in the air.
                motorBack.motorSpeed = Mathf.Lerp(
                    motorBack.motorSpeed,
                    -motorPower,
                    Time.deltaTime * 1.4f
                );
                if (speed < 4.3f)
                {
                    wheelParticle.transform.position = particlePosition.position;

                    em.enabled = true;
                }
                else
                    em.enabled = false;
            }
            else
                em.enabled = false;
        }
        else
        { // Move backward based on the input.
            if (Input.GetAxis("Horizontal") < 0 || HoriTemp < 0)
            {
                if (speed < -maxSpeed)
                {
                    if (isGrounded)
                        motorBack.motorSpeed = Mathf.Lerp(
                            motorBack.motorSpeed,
                            0,
                            Time.deltaTime * 3f
                        );
                }
                else
                {
                    if (isGrounded)
                        motorBack.motorSpeed = Mathf.Lerp(
                            motorBack.motorSpeed,
                            motorPower,
                            Time.deltaTime * 1.4f
                        );
                }
            }
            else
            { // Releasing car throttle and brake
                if (isGrounded)
                    motorBack.motorSpeed = Mathf.Lerp(
                        motorBack.motorSpeed,
                        0,
                        Time.deltaTime * decelerationSpeed
                    );
            }
        }

        // Update WheelJoint2D motor inputs

        motorWheel.motor = motorBack;

        // Check if the car can rotate, and if so, apply a rotate force to the car based
        // on the RotateForce variable.
        Rotate();

        // Call the EngineSoundEditor() or EngineSoundMobile() function depending on whether
        // the script is running in the Unity Editor or on a mobile device.
#if UNITY_EDITOR
        EngineSoundEditor();
#else
        EngineSoundMobile();
#endif

        if (!isMobile)
            HoriTemp = Input.GetAxis("Horizontal");

        if (useSmoke)
        {
            if (Input.GetAxis("Horizontal") > 0 || HoriTemp > 0)
            {
                if (speed < smokeTargetSpeed)
                    emSmoke.enabled = true;
                else
                    emSmoke.enabled = false;
            }
            else
                emSmoke.enabled = false;
        }
    }

    /**
     * This method is called after all other update methods have been called.
     *
     * @return	void
     */
    void LateUpdate()
    {
        // Retrieve the Rigidbody2D component attached to the GameObject, and then retrieve
        // its velocity vector. The magnitude property of a vector gives the magnitude
        // (length) of the vector.
        speed = GetComponent<Rigidbody2D>().velocity.magnitude;
        // Check the value of the horizontal axis. If either of these conditions is true,
        // it means the car is moving in a negative direction on the horizontal axis.
        if (Input.GetAxis("Horizontal") < 0 || HoriTemp < 0)
            speed = -speed;
    }

    /**
     * This method "Rotate" rotates the car game object based on the player's horizontal axis
     * input and the value of HoriTemp. The direction and amount of rotation is determined by
     * the value stored in the "RotateForce" variable.
     *
     * @return	void
     */
    void Rotate()
    {
        //based on player forward input(Like Hill Climb Racing game)
        if (Input.GetAxis("Horizontal") > 0 || HoriTemp > 0)
        {
            // Add "RotateForce" torque to the 2D rigidbody component of the game object to
            // rotate it in the forward direction.
            GetComponent<Rigidbody2D>()
                .AddTorque(RotateForce);
        }
        else
        {
            if (Input.GetAxis("Horizontal") < 0.0f || HoriTemp < 0)
                GetComponent<Rigidbody2D>().AddTorque(-RotateForce);
        }
    }

    /**
     * This coroutine defines the behavior of a Raycast that is used to determine
     * if a car object is on the ground or not.
     *
     * @return	void
     */
    IEnumerator RaycCast()
    {
        while (true)
        {
            // Wait until the end of the current frame before continuing to make sure that
            // the car's position is updated before the Raycast is performed.
            yield return new WaitForEndOfFrame();

            // Perform a 2D Raycast in Unity, starting at the position of the transform
            // (the position of the car) and pointing in the negative y direction (downwards),
            // with a maximum distance of 1000 units.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 1000);

            // Calculate the absolute value of the difference between the y position of the
            // hit point and the y position of the car.
            float distance = Mathf.Abs(hit.point.y - transform.position.y);

            if (distance < groundDistance)
                isGrounded = true;
            else
                isGrounded = false;

            // If the car is on the ground, "canRotate" will be false, and if the car is not
            // on the ground, "canRotate" will be true.
            canRotate = !isGrounded;
        }
    }

    public void EngineSoundMin()
    {
        if (EngineSoundS.pitch > minP)
            EngineSoundS.pitch -= 1.4f * Time.deltaTime;
    }

    public void EngineSoundMobile()
    {
        if (speed < 40)
        {
            EngineSoundS.pitch = Mathf.Lerp(
                EngineSoundS.pitch,
                Mathf.Clamp(HoriTemp * Multiplyer, minP, maxP),
                Time.deltaTime * 5
            );
        }
        else
        {
            EngineSoundS.pitch = Mathf.Lerp(
                EngineSoundS.pitch,
                Mathf.Clamp(HoriTemp * Multiplyer, minP, maxP),
                Time.deltaTime * 5
            );
        }
    }

    public void EngineSoundEditor()
    {
        if (speed < 40)
        {
            EngineSoundS.pitch = Mathf.Lerp(
                EngineSoundS.pitch,
                Mathf.Clamp(Input.GetAxis("Horizontal") * Multiplyer, minP, maxP),
                Time.deltaTime * 5
            );
        }
        else
        {
            EngineSoundS.pitch = Mathf.Lerp(
                EngineSoundS.pitch,
                Mathf.Clamp(Input.GetAxis("Horizontal") * Multiplyer, minP, maxP),
                Time.deltaTime * 5
            );
        }
    }

    /**
     * Vehicle input system for car Acceleration UI Button
     *
     * @access	public
     * @return	void
     */
    public void Acceleration()
    {
        HoriTemp = 1f;
    }

    /**
     * this function is used for car Brake\Backward UI Button.
     *
     * @access public
     * @return	void
     */
    public void Brake()
    {
        HoriTemp = -1f;
    }

    /**
     * this is for when player both release Brake or Acceleration button.
     *
     * @access public
     * @return	void
     */
    public void GasBrakeRelease()
    {
        HoriTemp = 0;
    }

    /**
     * A function used for needles rotations.
     *
     * @access public
     * @return	mixed
     */
    public float GetInput()
    {
        return motorBack.motorSpeed;
    }
}
