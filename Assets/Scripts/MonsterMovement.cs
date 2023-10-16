
using UnityEngine;
using System.Collections;

public class MonsterMovementController : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    private Vector3 pointA;
    private Vector3 pointB;
    private Vector3 nextPoint;
    private bool movingToB = true;

    public RoomController roomController;

    IEnumerator Start()
    {
        var pointA = transform.position;
        while (true)
        {
            // Calculate a random pointB within a 1-unit radius of the current position
            pointB = pointA + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

            // Ensure the next point is not outside the room boundaries
            pointB.x = Mathf.Clamp(pointB.x, pointA.x - 1, pointA.x + 1);
            pointB.y = Mathf.Clamp(pointB.y, pointA.y - 1, pointA.y + 1);

            // Start moving towards pointB
            nextPoint = pointB;
            movingToB = true;
            yield return StartCoroutine(MoveToNextPoint());

            if (!IsTileFloorAtPosition(nextPoint))
            {
                // Change direction instead of heading back to point A
                Vector3 temp = pointA;
                pointA = pointB;
                pointB = temp;
                movingToB = !movingToB;
                nextPoint = movingToB ? pointB : pointA;
                yield return StartCoroutine(MoveToNextPoint());
            }
        }
    }

    IEnumerator MoveToNextPoint()
    {
        float distance = Vector3.Distance(transform.position, nextPoint);
        float duration = distance / moveSpeed;

        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            float journey = (Time.time - startTime) / duration;
            transform.position = Vector3.Lerp(transform.position, nextPoint, journey);
            yield return null;
        }
    }

    bool IsTileFloorAtPosition(Vector3 position)
    {
        // Get all objects with the type "floorcontroller" at the given position
        FloorController[] controllers = GameObject.FindObjectsOfType<FloorController>();

        foreach (FloorController controller in controllers)
        {
            // Check if the object is not an instance of FloorController at the specified position
            if (controller.transform.position == position)
            {
                return false; // Yes There is a FloorController at the position
            }
        }

        return true; // FloorController found at the position
    }
}
