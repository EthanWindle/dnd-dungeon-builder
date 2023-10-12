using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;


public class ChatControls : MonoBehaviourPun
{
    public GameObject ChatMessage;
    public Transform ChatLog;

    public void AddNewChat(TMP_InputField input){
        string chat = input.text;
        string message = "[" + GlobalVariables.GetUserName() + "]";
        if(PhotonNetwork.IsMasterClient){
            message += "[DM]";
        }
        message += ":" + chat;

        GameObject newChat = PhotonNetwork.Instantiate(ChatMessage.name, new Vector2(0, 0), Quaternion.identity);
        newChat.GetComponent<TMP_Text>().text = message;
        newChat.transform.parent = ChatLog;

        input.text = "";
    }
}
