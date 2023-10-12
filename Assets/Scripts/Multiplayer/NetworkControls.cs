using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Chat;

public class NetworkControls : MonoBehaviour
{

    public GameObject leave;
    public GameObject end;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (PhotonNetwork.IsMasterClient){
            end.SetActive(true);
        } else {
            leave.SetActive(true);
        }
    }

    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("multiplayerPage");
    }

    public void EndGame(){
        PhotonNetwork.CurrentRoom.IsOpen = false;
        SceneManager.LoadScene("multiplayerPage");
    }

    
    

}
