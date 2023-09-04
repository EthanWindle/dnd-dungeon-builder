using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Controller for a Tile.
 */
public class TileController : MonoBehaviour
{

    public void Init(float size)
    {
        Transform transform = gameObject.GetComponent<Transform>();
        transform.localScale = new Vector3(size,size,1);
      
    }
}
