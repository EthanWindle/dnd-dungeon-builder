using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public GameObject Display;
    public GameObject Loading;
    public GameObject ButtonControls;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("connect");
    }

    public override void OnConnectedToMaster(){
        PhotonNetwork.JoinLobby();
        Debug.Log("lobby");
    }

    public override void OnJoinedLobby(){
        ButtonControls.GetComponent<ButtonControls>().GetSavedGames();
        Display.SetActive(true);
        Loading.SetActive(false);
        Debug.Log("in");
    }

    public void Disconnect(){
        PhotonNetwork.LeaveLobby();
    }

    public override void OnLeftLobby(){
        PhotonNetwork.Disconnect();
    }
}
