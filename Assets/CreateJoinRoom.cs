using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class CreateJoinRoom : MonoBehaviourPunCallbacks
{

    public TMP_InputField nameInput;
    public TMP_InputField passwordInput;
    
    public void HostRoom(){
        PhotonNetwork.CreateRoom(nameInput.text);
    }

    public void JoinRoom(){
        PhotonNetwork.JoinRoom(nameInput.text);
    }

    public override void OnJoinedRoom(){
        PhotonNetwork.LoadLevel("GameScene");
    }
}
