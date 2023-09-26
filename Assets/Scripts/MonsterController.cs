using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : EntityController
{

    public Texture2D texture;

    public override bool canBeMovedByPlayer()
    {
        return false;
    }

    public void Awake()
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), textureSize);

        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
