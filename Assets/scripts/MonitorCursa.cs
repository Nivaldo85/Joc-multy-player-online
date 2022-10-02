using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class MonitorCursa : MonoBehaviourPunCallbacks
{
    public GameObject incepeCursa;//buton incepere cursa online
    public GameObject[] counDownItems;
    public GameObject[] spawnPos;
    public GameObject[] masini;
    private bool[] spawnOcupat = { false, false, false, false };
    GameObject car;
    // semafor boolean pentru inceput cursa
    public static bool racing=false;
    // Start is called before the first frame update
    void Start()
    {
        incepeCursa.SetActive(false) ;
        Vector3 startPos = spawnPos[0].transform.position;
        Quaternion startRot = spawnPos[0].transform.rotation;
        
        if (PhotonNetwork.IsConnected)//aici e codul pentru online
        {
            startPos = spawnPos[PhotonNetwork.CurrentRoom.PlayerCount - 1].transform.position;//pozitia e functie de cati jucatori in camera
            startRot = spawnPos[PhotonNetwork.CurrentRoom.PlayerCount - 1].transform.rotation;

            if (JucatorRetea.InstantaJucatorLocal == null)
            {
                //aici instantiem masina in retea, apare si la ceilalti jucatori
                Debug.Log(masini[PhotonNetwork.CurrentRoom.PlayerCount - 1].name);
                car = PhotonNetwork.Instantiate(masini[PhotonNetwork.CurrentRoom.PlayerCount - 1].name,startPos,startRot,0);
                car.GetComponentInChildren<Camera>().enabled = true;
                car.GetComponentInChildren<jucator>().enabled = true;
                spawnOcupat[PhotonNetwork.CurrentRoom.PlayerCount - 1] = true;
               // Debug.Log(spawnOcupat[PhotonNetwork.CurrentRoom.PlayerCount]);
                car.GetComponentInChildren<Condu>().enabled = true;
                car.GetComponentInChildren<antiruliu>().enabled = true;
                car.GetComponentInChildren<FlipCar>().enabled = true;

            }

            if (PhotonNetwork.IsMasterClient)
            {
                incepeCursa.SetActive(true);
            }
        }
        else { 
            
            IncepeJocul(); 
        }

        //facem elementele inactive ca sa dispara de pe ecran
        foreach(GameObject g in counDownItems)
        {
            g.SetActive(false) ;
            //incepem functia
           
        }
        
    }

    public void IncepeJocul2()

    {
        for (int i = PhotonNetwork.CurrentRoom.PlayerCount; i < 4; i++)
        {
            GameObject g = spawnPos[i];
            //car = Instantiate(masini[i]);
            car = PhotonNetwork.Instantiate(masini[i].name, g.transform.position, g.transform.rotation, 0);
            car.GetComponentInChildren<Condu>().enabled = true;
            car.GetComponentInChildren<antiruliu>().enabled = true;
            car.GetComponentInChildren<FlipCar>().enabled = true;
            car.GetComponentInChildren<AIController3>().enabled = true;
        }
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("IncepeJocul3",RpcTarget.All,null);
        }
    }

    [PunRPC]//asta bagi ca sa mearga metoda asta pe toate calculatoarele
    public void IncepeJocul()
    { 
        int nr = 0;
        foreach(GameObject g in spawnPos)
        {
            if (spawnOcupat[nr] == false)
            {
                car = Instantiate(masini[nr]);

                car.transform.position = g.transform.position;
                car.transform.rotation = g.transform.rotation;
                if (nr == 0)
                {
                    car.GetComponentInChildren<AIController3>().enabled = false;
                    car.GetComponentInChildren<jucator>().enabled = true;
                    car.GetComponentInChildren<Camera>().enabled = true;
                }
            }
            nr++;
        }
        StartCoroutine(PlayCountDownn());
        incepeCursa.SetActive(false);
    }
    IEnumerator PlayCountDownn()
    {
        //asteptam 2 secunde
        yield return new WaitForSeconds(2);
        //ciclam elementele 3 2 1 GO
        //cate o secunda pe ecran
        foreach (GameObject g in counDownItems)
        {
            g.SetActive(true);
            yield return new WaitForSeconds(1);
            g.SetActive(false);
        }
        //incepem cursa
        racing = true;

    }
    [PunRPC]//asta bagi ca sa mearga metoda asta pe toate calculatoarele
    public void IncepeJocul3()
    {
       
        StartCoroutine(PlayCountDownn());
        incepeCursa.SetActive(false);
    }
    


}
