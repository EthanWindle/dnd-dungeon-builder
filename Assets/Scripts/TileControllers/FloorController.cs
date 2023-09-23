using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/**
 * Controller for a Floor tile. Can always be entered.
 */
public class FloorController : TileController
{


    
    public Texture2D defaultFloor;
    public float differntFloorChance;
    public Texture2D[] textures;

    public override bool CanEnter()
    {
        return true;
    }

    public override void Init(float size)
    {
        System.Random random = new();

        Texture2D texture;

        if (random.NextDouble() < differntFloorChance)
        {
            texture = textures[random.Next(textures.Length)];
        }
        else
        {
            texture = defaultFloor;
        }

        
        Sprite sprite = Sprite.Create(texture, new Rect(0,0, texture.width, texture.height), new Vector2(0.5f, 0.5f), textureSize);

        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;

        base.Init(size);
    }

}
