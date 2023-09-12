using UnityEngine;

using System.IO;

[System.Serializable]
public class Recorder
{
    public int width;
    public int height;
    // Add other serializable fields here

    public Recorder(GridController gridController)
    {
        width = gridController.width;
        height = gridController.height;
        // Initialize other serializable fields here
    }
}

public class GridControllerJsonSerializer : MonoBehaviour
{
    public void SerializeToJson(GridController gridController, string filePath)
    {
        Recorder serializableGridController = new Recorder(gridController);

        string json = JsonUtility.ToJson(serializableGridController, true);
        File.WriteAllText(filePath, json);
    }
}