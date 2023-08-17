using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropController : MonoBehaviour
{
    public void Init(float size)
    {
        Transform transform = gameObject.GetComponent<Transform>();
        transform.localScale = new Vector3(size, size, 1);
    }
}
