using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circuit : MonoBehaviour
{
    public GameObject[] puncteDeTrecere;
    private void OnDrawGizmos()
    {
        DrawGizmos(false);
    }
    private void OnDrawGizmosSelected()
    {
        DrawGizmos(true);
    }
    void DrawGizmos(bool selectat) {
        if (selectat == false) return;
        if (puncteDeTrecere.Length > 1)
        {
            Vector3 anterior = puncteDeTrecere[0].transform.position;
            for(int i = 1; i < puncteDeTrecere.Length; i++)
            {
                Vector3 urmator = puncteDeTrecere[i].transform.position;
                Gizmos.DrawLine(anterior, urmator);
                anterior = urmator;
            }
            Gizmos.DrawLine(anterior, puncteDeTrecere[0].transform.position);
        }
    }
}
