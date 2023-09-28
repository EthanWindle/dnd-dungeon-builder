using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Controller for entities, players and monsters.
//This controller will be used for both as players and monsters share a lot of functionality,
//such as being moved by the mouse
public abstract class EntityController : MonoBehaviour
{

    public Texture2D texture;
    protected readonly int textureSize = 16;

    public virtual void Init(float size)
    {
        Transform transform = gameObject.GetComponent<Transform>();
        transform.localScale = new Vector3(size, size, 1);

    }

    public abstract bool canBeMovedByPlayer();

    public void Awake()
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), textureSize);

        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
