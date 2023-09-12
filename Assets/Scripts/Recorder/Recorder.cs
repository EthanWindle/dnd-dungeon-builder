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
