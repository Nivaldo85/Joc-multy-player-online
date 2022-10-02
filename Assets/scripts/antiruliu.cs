using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class antiruliu : MonoBehaviour
{
    public float fortaT = 5000.0f;
    public WheelCollider WFL;
    public WheelCollider WFR;
    public WheelCollider WBL;
    public WheelCollider WBR;
    public GameObject CG;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.centerOfMass = CG.transform.localPosition;
    }
    void Roti(WheelCollider rw, WheelCollider lw)
    {
        WheelHit hit;
        //distanta supensiei
        float travelL = 1.0f;
        float travelR = 1.0f;
        bool groundedL = lw.GetGroundHit(out hit);
        if (groundedL)
            //calculam procentajul din suspensie parcurs stanga
            travelL = (-lw.transform.InverseTransformPoint(hit.point).y - lw.radius) / lw.suspensionDistance;

        bool groundedR = rw.GetGroundHit(out hit);
        if (groundedR)
            //calculam procentajul din suspensie parcurs dreapta
            travelR = (-rw.transform.InverseTransformPoint(hit.point).y - rw.radius) / rw.suspensionDistance;
        //calculam forta care trebuie adaugata la roata opusa
        float fortaA = (travelL - travelR) * fortaT;
        if (groundedL) 
            rb.AddForceAtPosition(lw.transform.up * fortaA, lw.transform.position);
        if (groundedR)
            rb.AddForceAtPosition(rw.transform.up * fortaA, rw.transform.position);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Roti(WFR, WFL);
        Roti(WBL, WBL);
    }
}
