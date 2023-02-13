using UnityEngine;
using System.Collections;

public class CarInput : MonoBehaviour
{
    [HideInInspector]
    [field: SerializeField]
    CarController carController;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(.3f);
        carController = GameObject.FindObjectOfType<CarController>();
    }

    public void Gas()
    {
        carController.Acceleration();
    }

    public void Brake()
    {
        carController.Brake();
    }

    public void ReleaseGasBrake()
    {
        carController.GasBrakeRelease();
    }
}
