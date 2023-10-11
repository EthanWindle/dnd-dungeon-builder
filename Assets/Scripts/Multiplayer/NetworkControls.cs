using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Chat;

public class NetworkControls : MonoBehaviour, IChatClientListener
{

    public GameObject leave;
    public GameObject end;

    private ChatClient chat;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (PhotonNetwork.IsMasterClient){
            end.SetActive(true);
        } else {
            leave.SetActive(true);
        }
        chat = new ClientChat(this);
    }

    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("multiplayerPage");
    }

    public void EndGame(){
        PhotonNetwork.CurrentRoom.IsOpen = false;
        SceneManager.LoadScene("multiplayerPage");
    }

    
    

    //Chat Functions
    public void DebugReturn(DebugLevel level, string message){
        throws new System.NotImplementedException();
    }
    public void OnChatStateChange(ChatState state){
        throws new System.NotImplementedException();
    }
    public void OnConnected(){
        throws new System.NotImplementedException();
    }
    public void OnDisconnected(){
        throws new System.NotImplementedException();
    }
    public void OnGetMessages(string channelName, string[] senders, object[] messages){
        throws new System.NotImplementedException();
    }
    public void OnGetPrivateMessage(string sender, object message, string channelName){
        throws new System.NotImplementedException();
    }
    public void OnStatusUpdate(string sender, int status, bool, gotMessage, object message){
        throws new System.NotImplementedException();
    }
    public void OnSubscribed(string[] channels, bool[] results){
        throws new System.NotImplementedException();
    }
    public void OnUnsubscribed(){
        throws new System.NotImplementedException(string[] channels);
    }
    public void OnUserSubscribed(string channel, string user){
        throws new System.NotImplementedException();
    }
    public void OnUserUnsubscribed(){
        throws new System.NotImplementedException(string channel, string user);
    }
}
