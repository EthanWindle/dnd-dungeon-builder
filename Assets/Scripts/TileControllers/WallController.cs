using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Controller for a wall. Can never be entered.
 */
public class WallController : TileController
{
    public override bool CanEnter()
    {
        return false;
    }
    //Takes an array of adjacent tiles, of length eight in the format:
    //2 4 7
    //1 x 6
    //0 3 5
    public void SetTexture(TileController[] adjacentTiles, SpritesheetManager sm)
    {
        Sprite sprite;



        if (CheckLocations(adjacentTiles, 1, 2, 4)) 
        {
            sprite = sm.Get("Small Bottom Right");
        }
        else if (CheckLocations(adjacentTiles, 4, 7, 6))
        {
            sprite = sm.Get("Small Bottom Left");

        }
        else if (CheckLocations(adjacentTiles, 1, 0, 3))
        {
            sprite = sm.Get("Small Top Right");
        }
        else if (CheckLocations(adjacentTiles, 3, 5, 6))
        {
            sprite = sm.Get("Small Top Left");
        }
        else if (CheckLocations(adjacentTiles,1, 2) || CheckLocations(adjacentTiles, 1, 0))
        {
            sprite = sm.Get("Right");
        }
        else if (CheckLocations(adjacentTiles, 6, 5) || CheckLocations(adjacentTiles, 6, 7))
        {
            sprite = sm.Get("Left");
        }
        else if (CheckLocations(adjacentTiles, 4, 2) || CheckLocations(adjacentTiles, 4, 7))
        {
            sprite = sm.Get("Bottom");
        }
        else if (CheckLocations(adjacentTiles, 3, 0) || CheckLocations(adjacentTiles, 3, 5))
        {
           sprite = sm.Get("Top");
        }
        else if (CheckLocations(adjacentTiles, 2))
        {
            sprite = sm.Get("Bottom Right");
        }
        else if (CheckLocations(adjacentTiles, 7))
        {
           sprite = sm.Get("Bottom Left");
        }
        else if (CheckLocations(adjacentTiles, 0))
        {
            sprite = sm.Get("Top Right");
        }
        else if (CheckLocations(adjacentTiles, 5))
        {
            sprite =sm.Get("Top Left");
        }

        else
        {
            sprite = sm.Get("Bottom");
        }

  

        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;

    }

    private bool CheckLocations(TileController[] adjacentTiles, params int[] indices)
    {
        for (int i = 0; i <  indices.Length; i++)
        {
            if (adjacentTiles[indices[i]] is not FloorController)
            {
                return false;
            }
        }
        return true;
    }

}

