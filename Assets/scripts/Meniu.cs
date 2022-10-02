using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;


public class Meniu : MonoBehaviourPunCallbacks
{
    byte nrMaximJucatori = 4;
    bool seConecteaza;
    public Text feedBackText;
    string versiuneJoc = "1";

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public void ConnectNetwork()
    {
        feedBackText.text = "";
        seConecteaza = true;
        PhotonNetwork.NickName = "Marius";
        if (PhotonNetwork.IsConnected)
        {
            feedBackText.text += "\n intram in camera...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            feedBackText.text += "\n ne conectam....";
            PhotonNetwork.GameVersion = versiuneJoc;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //Incarcam scena cu nr de ordine 1
    //Scena meniu are nr de ordine 0, din ea incepe jocul
    public void SinglePlayer()
    {
        SceneManager.LoadScene(1);
    }
    //Parasim aplicatia
    public void QuitGame()
    {
        Application.Quit();

    }
    ////Calbackuri de retea
    public override void OnConnectedToMaster()
    {
        if (seConecteaza)
        {
            feedBackText.text += "\n Se conecteaza la master";
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        feedBackText.text+="\n nu ne-am conectat";
        PhotonNetwork.CreateRoom(null,new RoomOptions { MaxPlayers = this.nrMaximJucatori });
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        feedBackText.text += "\n ne-am deconectat pentru ca " + cause;
        seConecteaza = false;
    }
    public override void OnJoinedRoom()
    {
        feedBackText.text = "\n Ne-am conectat la camera cu " + PhotonNetwork.CurrentRoom.PlayerCount + " jucatori.";
        PhotonNetwork.LoadLevel("SampleScene");
    }
}
