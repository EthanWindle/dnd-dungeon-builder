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

    private static int roomCount = 25;
    public static void setRoomCount(int value){
        roomCount = value;
    }
    public static int getRoomCount(){
        return roomCount;
    }

    private static bool monsters = true;
    public static void setMonsters(bool value){
        monsters = value;
    }
    public static bool hasMonsters(){
        return monsters;
    }

    private static bool props = true;
    public static void setProps(bool value){
        props = value;
    }
    public static bool hasProps(){
        return props;
    }
}
