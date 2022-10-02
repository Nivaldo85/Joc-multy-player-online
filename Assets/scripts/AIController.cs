using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public circuit cir;
    Condu c;
    public float sensibilitateVolan=0.01f;
    Vector3 tinta;
    int punctTrecereCurent = 0;

    // Start is called before the first frame update
    void Start()
    {
        c = this.GetComponent<Condu>();
        tinta = cir.puncteDeTrecere[punctTrecereCurent].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tintaLocala = c.rb.gameObject.transform.InverseTransformPoint(tinta);
        float distantaLaTinta = Vector3.Distance(tinta, c.rb.gameObject.transform.position);

        float unghTinta = Mathf.Atan2(tintaLocala.x, tintaLocala.z) * Mathf.Rad2Deg;
        float coteste = Mathf.Clamp(unghTinta * sensibilitateVolan, -1, 1) * Mathf.Sign(c.currentSpeed);
        float accel = 0.5f;
        float frana = 0;
        c.Go(accel, coteste, frana);
        if (distantaLaTinta < 2)
        {
            punctTrecereCurent++;
            if (punctTrecereCurent >= cir.puncteDeTrecere.Length)
            {
                punctTrecereCurent = 0;
                tinta = cir.puncteDeTrecere[punctTrecereCurent].transform.position;
            }
        }
    }
}
