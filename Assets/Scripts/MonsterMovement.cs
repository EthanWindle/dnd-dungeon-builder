
using UnityEngine;
using System.Collections;

public class MonsterMovementController : MonoBehaviour
{
    public Vector3 pointB;

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
        }
    }

    IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        var i = 0.0f;
        var rate = 1.0f / time;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
    }
}
