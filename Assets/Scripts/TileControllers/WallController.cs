using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Controller for a wall. Can never be entered.
 */
public class WallController : TileController
{


    public Texture2D defaulWall;

    public Texture2D rightWall;
    public Texture2D leftWall;
    public Texture2D topWall;
    public Texture2D bottomWall;

    public Texture2D topLeftWall;
    public Texture2D topRightWall;
    public Texture2D bottomLeftWall;
    public Texture2D bottomRightWall;

    public Texture2D smallTopLeftWall;
    public Texture2D smallTopRightWall;
    public Texture2D smallBottomLeftWall;
    public Texture2D smallBottomRightWall;

    public override bool CanEnter()
    {
        return false;
    }
    //Takes an array of adjacent tiles, of length eight in the format:
    //2 4 7
    //1 x 6
    //0 3 5
    public void SetTexture(TileController[] adjacentTiles)
    {
        Texture2D texture;
        

        if (CheckLocations(adjacentTiles, 1, 2, 4)) 
        {
            texture = smallBottomRightWall;
        }
        else if (CheckLocations(adjacentTiles, 4, 7, 6))
        {
            texture = smallBottomLeftWall;
        }
        else if (CheckLocations(adjacentTiles, 1, 0, 3))
        {
            texture = smallTopRightWall;
        }
        else if (CheckLocations(adjacentTiles, 3, 5, 6))
        {
            texture = smallTopLeftWall;
        }
        else if (CheckLocations(adjacentTiles,1, 2) || CheckLocations(adjacentTiles, 1, 0))
        {
            texture = rightWall;
        }
        else if (CheckLocations(adjacentTiles, 6, 5) || CheckLocations(adjacentTiles, 6, 7))
        {
            texture = leftWall;
        }
        else if (CheckLocations(adjacentTiles, 4, 2) || CheckLocations(adjacentTiles, 4, 7))
        {
            texture = bottomWall;
        }
        else if (CheckLocations(adjacentTiles, 3, 0) || CheckLocations(adjacentTiles, 3, 5))
        {
            texture = topWall;
        }
        else if (CheckLocations(adjacentTiles, 2))
        {
            texture = bottomRightWall;
        }
        else if (CheckLocations(adjacentTiles, 7))
        {
            texture = bottomLeftWall;
        }
        else if (CheckLocations(adjacentTiles, 0))
        {
            texture = topRightWall;
        }
        else if (CheckLocations(adjacentTiles, 5))
        {
            texture =topLeftWall;
        }

        else
        {
            texture = defaulWall;
        }

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), textureSize);

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

