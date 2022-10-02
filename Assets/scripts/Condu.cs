using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condu : MonoBehaviour
{
    public WheelCollider[] WCs;
    public GameObject[] Wheels;
    public float torque = 200;
    public float maxSteerAngle = 30;
    public float maxBrakeTorque = 500;
    public Rigidbody rb;
    public float currentSpeed { get { return rb.velocity.magnitude ; } }
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    public void Go(float accel, float steer, float brake)
    {
        accel = Mathf.Clamp(accel, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
        brake = Mathf.Clamp(brake, 0, 1) * maxBrakeTorque;

        float thrustTorque = accel * torque;

        for (int i = 0; i < 4; i++)
        {
            WCs[i].motorTorque = thrustTorque;

            if (i < 2)
                WCs[i].steerAngle = steer;
            else
                WCs[i].brakeTorque = brake;

            Quaternion quat;
            Vector3 position;
            WCs[i].GetWorldPose(out position, out quat);
            Wheels[i].transform.position = position;
            Wheels[i].transform.localRotation = quat;
        }
    }

    // Update is called once per frame
    
}

