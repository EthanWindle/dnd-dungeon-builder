using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RoomPlacementTests
{
    private GameObject gridControllerObject;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        // Load the test scene with your GridController and other necessary objects
        yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("YourTestScene");

        // Find the GridController GameObject in the scene
        gridControllerObject = GameObject.Find("GridController");
        Assert.IsNotNull(gridControllerObject, "GridController not found in the scene.");
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        // Unload the test scene
        yield return UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("YourTestScene");
    }

    [UnityTest]
    public IEnumerator RoomIsPlacedAtOrigin()
    {
        // Assuming you have a public method in GridController to place a room, e.g., PlaceRoomAtOrigin()
        GridController gridController = gridControllerObject.GetComponent<GridController>();
        Assert.IsNotNull(gridController, "GridController component not found on the GridController GameObject.");

        // Perform the room placement
        gridController.PlaceRoomAtOrigin();

        // You may need to yield some frames to allow the room placement to complete (use WaitForSeconds or yield return null)

        // Assert that a room is placed at (0, 0)
        RoomController roomController = gridController.GetRoomAt(0, 0);
        Assert.IsNotNull(roomController, "Room not placed at (0, 0).");

        // Additional assertions can be added to check the room's properties or state

        yield return null;
    }
}
