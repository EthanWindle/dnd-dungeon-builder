
using UnityEngine;
using System.Collections;

public class MonsterMovementController : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    private Vector3 pointA;
    private Vector3 pointB;
    private Vector3 nextPoint;
    private bool movingToB = true;

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

            // Switch direction and move back towards pointA
            nextPoint = pointA;
            movingToB = false;
            yield return StartCoroutine(MoveToNextPoint());
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
}
