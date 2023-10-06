using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/**
 * Controller for a Floor tile. Can always be entered.
 */
public class FloorController : TileController
{


    
    public float differntFloorChance;

    public override bool CanEnter()
    {
        return true;
    }

    public void Init(float size, SpritesheetManager sm)
    {
        System.Random random = new();

        Sprite sprite;

        if (random.NextDouble() < differntFloorChance)
        {
            int floorIndex = random.Next(1, 5);
            sprite = sm.Get("Floor"+floorIndex);
        }
        else
        {
            sprite = sm.Get("Floor");
        }

        
        

        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;

        base.Init(size);
    }

}
