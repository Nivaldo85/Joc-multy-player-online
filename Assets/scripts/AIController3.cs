using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController3 : MonoBehaviour
{
    private circuit circuit;
    public float brakingSensitivity = 3f;
    Condu ds;
    public float steeringSensitivity = 0.01f;
    public float accelSensitivity = 0.3f;
    public float lastTimeMoving = 0;

    Vector3 target;
    Vector3 nextTarget;
    int currentWP = 0;
    float totalDistanceToTarget;
    Rigidbody rb;

    GameObject tracker;
    int currentTrackerWP = 0;
    float lookAhead = 10;


    // Start is called before the first frame update
    void Start()

    {
        if (circuit == null)
            circuit = GameObject.FindGameObjectWithTag("Circuit").GetComponent<circuit>();
        rb = this.GetComponent<Rigidbody>();
        ds = this.GetComponent<Condu>();
        target = circuit.puncteDeTrecere[currentWP].transform.position;
        nextTarget = circuit.puncteDeTrecere[currentWP + 1].transform.position;
        //totalDistanceToTarget = Vector3.Distance(target, ds.rb.gameObject.transform.position);

        tracker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        DestroyImmediate(tracker.GetComponent<Collider>());
        tracker.transform.position = ds.rb.gameObject.transform.position;
        tracker.transform.rotation = ds.rb.gameObject.transform.rotation;
        tracker.GetComponent<MeshRenderer>().enabled=false;
        
    }


    void ProgressTracker()
    {
        tracker.GetComponent<MeshRenderer>().enabled = false;
        Debug.DrawLine(ds.rb.gameObject.transform.position, tracker.transform.position);

        if (Vector3.Distance(ds.rb.gameObject.transform.position, tracker.transform.position) > lookAhead) return;

        tracker.transform.LookAt(circuit.puncteDeTrecere[currentTrackerWP].transform.position);
        tracker.transform.Translate(0, 0, 1.0f);  //speed of tracker

        if (Vector3.Distance(tracker.transform.position, circuit.puncteDeTrecere[currentTrackerWP].transform.position) < 1)
        {
            currentTrackerWP++;
            if (currentTrackerWP >= circuit.puncteDeTrecere.Length)
                currentTrackerWP = 0;
        }

    }
    void ResetLayer()
    {
        ds.rb.gameObject.layer = 0;
    }
    // Update is called once per frame
    bool isJump = false;
    void Update()
    {
        if (!MonitorCursa.racing)
        {
            lastTimeMoving = Time.time;
            return;
        }
        ProgressTracker();
        if (ds.rb.velocity.magnitude > 1)
        {
            lastTimeMoving = Time.time;
        }
        if (Time.time > lastTimeMoving + 4)
        {
            //respawn
            ds.rb.gameObject.transform.position = circuit.puncteDeTrecere[currentTrackerWP-1].transform.position
                +Vector3.up*2+//adaugam inaltime ca sa nu spawnam in pista
                new Vector3(Random.Range(-1,1),0,Random.Range(-1,1)); //adaugam element aleator la locul unde se spawn-eaza
            tracker.transform.position = ds.rb.gameObject.transform.position;// schimbam si pozitia iepurelui
            ds.rb.gameObject.layer = 9;//schimbam layerul in respawn
            Invoke("ResetLayer", 6);//invocam functia de schimbare a layarului default

        }

        Vector3 localTarget = ds.rb.gameObject.transform.InverseTransformPoint(tracker.transform.position);
        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        float steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(ds.currentSpeed);

        float brake = 0;
        float accel = 1f;
        if (tracker.transform.rotation != ds.transform.rotation&&rb.velocity.magnitude>15) { accel = 0;brake = 1; }
        if (steer > 0.2) accel=accel / 8;
        if (steer < 0.1&&rb.velocity.magnitude<10f) { accel = 1; brake = 0; }
        ds.Go(accel, steer, brake);

        
    }
}