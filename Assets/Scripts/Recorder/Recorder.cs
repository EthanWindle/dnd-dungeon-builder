using UnityEngine;
using System.Collections.Generic;

using System.IO;

[System.Serializable]
public class Recorder
{
    public int width;
    public int height;
    public float cellSize;
    public float cellSpacing;

    public List<RecorderTile> tiles = new List<RecorderTile>();

    public Recorder(GridController gridController)
    {
        width = gridController.width;
        height = gridController.height;
        cellSize = gridController.cellSize;
        cellSpacing = gridController.cellSpacing;    
    }

    public void AddTile(RecorderTile tile)
    {
        tiles.Add(tile);
    }
}

public class GridControllerJsonSerializer : MonoBehaviour
{
    public static void SerializeToJson(GridController gridController, string filePath, Recorder serializableGridController)
    {
        string json = JsonUtility.ToJson(serializableGridController, true);
        File.WriteAllText(filePath, json);
    }
}