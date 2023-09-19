using UnityEngine;

using System.IO;

[System.Serializable]
public class Recorder
{
    public int width;
    public int height;
    // Add other serializable fields here
    public float cellSize;
    public float cellSpacing;

    //private GameObject[,] backgroundLayer; //Layer for tiles like walls, floors, doors.
    //private GameObject[,] foregroundLayer; //Layer for props and entities like players and monsters

    public GameObject[] rooms;

    public Recorder(GridController gridController)
    {
        width = gridController.width;
        height = gridController.height;
        // Initialize other serializable fields here
        cellSize = gridController.cellSize;
        cellSpacing = gridController.cellSpacing;
        rooms = gridController.rooms;
        
    }
}

public class GridControllerJsonSerializer : MonoBehaviour
{
    public static void SerializeToJson(GridController gridController, string filePath)
    {
        Recorder serializableGridController = new Recorder(gridController);
        
        string json = JsonUtility.ToJson(serializableGridController, true);
        File.WriteAllText(filePath, json);
    }
}