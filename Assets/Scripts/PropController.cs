using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Controller for a Prop.
 */
public class PropController : MonoBehaviour
{

    protected readonly int textureSize = 24;

    public Texture2D texture;

    public void Init(float size)
    {
        Transform transform = gameObject.GetComponent<Transform>();
        transform.localScale = new Vector3(size, size, 1);

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), textureSize);

        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;

    }
}
