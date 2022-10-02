using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SINE : MonoBehaviour
{
    public circuit circuit;
    Vector3 target;
    //punctul de control curent
    int currentWP = 0;
    //viteza iepurelui
    float speed = 20.0f;
    //cat de aproape trebuie sa treaca de puntul de control pentru a se considera atins
    float accuracy = 1.0f;
    //viteza de rotatie catre puncte de control a iepurelui
    float rotSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        //incarcam circuitul cu punctele de control
        target = circuit.puncteDeTrecere[currentWP].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //distanta pana la punctul de control
        float distanceToTarget = Vector3.Distance(target, this.transform.position);
        //directia
        Vector3 direction = target - this.transform.position;
  
        //ne rotim inspre punctul de control
        this.transform.LookAt(target);
        //ne deplasam inspre punctul de control
        this.transform.Translate(0, 0, speed * Time.deltaTime);
        //acest if verifica daca am trecut de un punct de control
        //si trecem la urmatorul
        if (distanceToTarget < accuracy)
        {
            currentWP++;
            if (currentWP >= circuit.puncteDeTrecere.Length)
                currentWP = 0;
            target = circuit.puncteDeTrecere[currentWP].transform.position;
        }
    }
}
