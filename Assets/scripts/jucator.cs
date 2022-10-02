using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jucator : MonoBehaviour
{
    private Condu c;
    float lastTimeMoving = 0;
    Vector3 lastPOsition;
    Quaternion lastRotation;

    void ResetLayer()
    {
        c.rb.gameObject.layer = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        c = this.GetComponent<Condu>();
    }

    // Update is called once per frame
    void Update()
    {
        //acceleratie
        float a = Input.GetAxis("Vertical");
        //virare roti
        float s = Input.GetAxis("Horizontal");
        //frana
        float b = Input.GetAxis("Jump");
        //verificam daca masina nu s-a blocat
        //si cursa a inceput
        if(c.rb.velocity.magnitude>1|| !MonitorCursa.racing)
        {
            lastTimeMoving = Time.time;
        }
        //Salvam locatia si rotatia masinii cand aceasta se misca si cursa a inceput
        RaycastHit hit;
        if(Physics.Raycast(c.rb.gameObject.transform.position,-Vector3.up,out hit, 10))
        {
            if(hit.collider.gameObject.tag == "Drum")
            {
                lastPOsition = c.rb.gameObject.transform.position;
                lastRotation = c.rb.gameObject.transform.rotation;
            }
        }
        //Daca am depasit 4 secunde, de cand nu ama mai inregistrat poazitia si locatia masiinii
        //Resetam la ultimele cunoscute
        if (Time.time > lastTimeMoving + 4)
        {
            c.rb.gameObject.transform.position = lastPOsition;
            c.rb.gameObject.transform.rotation = lastRotation;
            c.rb.gameObject.layer = 9;
        }
        //Blpcam masina, daca nu a inceput cursa
        if (!MonitorCursa.racing) a = 0;
        //Merge masina
        c.Go(a*0.4f, s*0.3f, b*3);
    }
}
