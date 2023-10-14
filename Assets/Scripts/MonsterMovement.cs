using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovementController : MonoBehaviour
{
    private Transform monsterTransform;
    private Vector3 targetPosition;
    private float moveSpeed = 2.0f; // Adjust the movement speed as needed
    public float cellsize;
    public float cellspacing;
    public RoomController roomController;


    private void Start()
    {
        monsterTransform = transform;
        // Set the initial target position (where the monster should move to)
        SetRandomTargetPosition();
    }

    private void Update()
    {
        // Move the monster towards the target position
        monsterTransform.position = Vector3.MoveTowards(monsterTransform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Check if the monster has reached the target position
        if (Vector3.Distance(monsterTransform.position, targetPosition) < 0.1f)
        {
            // Set a new random target position
            SetRandomTargetPosition();
        }
    }

    private void SetRandomTargetPosition()
    {
        if (IsTargetPositionValid(targetPosition) == true)
        {
            // Generate a random position within the room boundaries
            float randomX = Random.Range(roomController.GetX(), roomController.GetX() + roomController.width);
            float randomY = Random.Range(roomController.GetY(), roomController.GetY() + roomController.height);
            targetPosition = new Vector3(randomX, randomY, monsterTransform.position.z);
        }
    }

    private bool IsTargetPositionValid(Vector3 position)
    {
        // Calculate grid coords
        int xIndex = Mathf.FloorToInt((position.x - roomController.GetX()) / cellsize);
        int yIndex = Mathf.FloorToInt((position.y - roomController.GetY()) / cellsize);
        TileController tile;

        // Check if the calculated indices are within the bounds of the room
        if (xIndex >= 0 && xIndex < roomController.width && yIndex >= 0 && yIndex < roomController.height)
        {
            // Get the tile at the calculated indices
            for (int x = 0; x < roomController.width; x++)
            {
                for (int y = 0; y < roomController.height; y++)
                {
                    tile = roomController.GetComponent<TileController>();
                    if (tile != null && tile.CompareTag("Floor"))
                    {
                        return true;
                    }
                }
            }            
        }
        return false;
    }
}
