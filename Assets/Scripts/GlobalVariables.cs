using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public static class GlobalVariables
{
    private static string mapName;

    public static void setMapName(string name){
        mapName = "Assets/Saves/" + name + ".json";
    }

    public static string getMap(){
        return mapName;
    }

    public static void clearMap(){
        Debug.Log("clear");
        mapName = null;
    }

    private static string UserName;
    public static void SetUserName(string name){
        UserName = name;
    }
    public static string GetUserName(){
        return UserName;
    }
}
