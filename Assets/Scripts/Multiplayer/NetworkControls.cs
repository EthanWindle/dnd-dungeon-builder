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
        chat.Connect(PhotonNetwork.PhotonServerSettings.AppSetings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues());
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
    void DebugReturn(DebugLevel Level, string message){
        throw new NotImplementedException();
    }
    public void OnChatStateChange(ChatState state){
        throw new NotImplementedException();
    }
    public void OnConnected(){
        throw new NotImplementedException();
    }
    public void OnDisconnected(){
        throw new NotImplementedException();
    }
    public void OnGetMessages(string channelName, string[] senders, object[] messages){
        throw new NotImplementedException();
    }
    public void OnPrivateMessage(string sender, object message, string channelName){
        throw new NotImplementedException();
    }
    public void OnStatusUpdate(string sender, int status, bool gotMessage, object message){
        throw new NotImplementedException();
    }
    public void OnSubscribed(string[] channels, bool[] results){
        throw new NotImplementedException();
    }
    public void OnUnsubscribed(string[] channels){
        throw new NotImplementedException();
    }
    public void OnUserSubscribed(string channel, string user){
        throw new NotImplementedException();
    }
    public void OnUserUnsubscribed(string channel, string user){
        throw new NotImplementedException();
    }
}
