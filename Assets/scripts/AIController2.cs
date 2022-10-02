using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController2 : MonoBehaviour
{
    public circuit circuit;
    Condu ds;
    public float steeringSensitivity = 0.01f;
    Vector3 target;
    int currentWP = 0;

    // Start is called before the first frame update
    void Start()
    {
        ds = this.GetComponent<Condu>();
        target = circuit.puncteDeTrecere[currentWP].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localTarget = ds.rb.gameObject.transform.InverseTransformPoint(target);
        float distanceToTarget = Vector3.Distance(target, ds.rb.gameObject.transform.position);

        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        float steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(ds.currentSpeed);
        float accel = 0.5f;
        float brake = 0;

        if ((distanceToTarget < 10)&&(ds.rb.velocity.magnitude>7f)) { brake = 0.9f; accel = 0.0f; }
        //Debug.Log(ds.rb.velocity.magnitude);
        ds.Go(accel, steer, brake);

        if (distanceToTarget < 4) //threshold, make larger if car starts to circle waypoint
        {
            currentWP++;
            if (currentWP >= circuit.puncteDeTrecere.Length)
                currentWP = 0;
            target = circuit.puncteDeTrecere[currentWP].transform.position;
        }

    }
}
