using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterMovementController : MonoBehaviour
{
    private Transform monsterTransform;
    private Vector3 targetPosition;
    private RoomController roomController;
    private float moveSpeed = 2.0f; // Adjust the movement speed as needed

    private void Start()
    {
        monsterTransform = transform;
        // Set the initial target position (where the monster should move to)
        SetRandomTargetPosition();
        roomController = GetComponent<RoomController>();
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
        // Generate a random position within the room boundaries (adjust as needed)
        float randomX = Random.Range(roomController.GetX(), roomController.GetX() + roomController.width);
        float randomY = Random.Range(roomController.GetY(), roomController.GetY() + roomController.height);
        targetPosition = new Vector3(randomX, randomY, monsterTransform.position.z);
    }
}
