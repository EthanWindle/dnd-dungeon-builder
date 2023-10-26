using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public static class GlobalVariables
{
    private static string mapName;
    private static int roomCount = 25;
    private static bool monsters = true;
    private static bool props = true;
    private static string theme;

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

    public static void setRoomCount(int value){
        roomCount = value;
    }
    public static int getRoomCount(){
        return roomCount;
    }

    public static void setMonsters(bool value){
        monsters = value;
    }
    public static bool hasMonsters(){
        return monsters;
    }

    public static void setProps(bool value){
        props = value;
    }
    public static bool hasProps(){
        return props;
    }

    public static void setTheme(string str){
        theme = str;
    }
    public static string getTheme(){
        return theme;
    }
}
