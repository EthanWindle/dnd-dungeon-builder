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

    public override bool CanEnter()
    {
        return false;
    }
    //Takes an array of adjacent tiles, of length eight in the format:
    //0 3 5
    //1 x 6
    //2 4 7
    public void SetTexture(TileController[] adjacentTiles)
    {
        Texture2D texture;

        //Is a right wall
        if (adjacentTiles[1] is FloorController &&  (adjacentTiles[0] is FloorController ||  adjacentTiles[2] is FloorController))
        {
            texture = rightWall;
        }
        //Is a right wall
        else if (adjacentTiles[6] is FloorController && (adjacentTiles[5] is FloorController ||  adjacentTiles[7] is FloorController))
        {
            texture = leftWall;
        }

        else
        {
            texture = defaulWall;
        }

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), textureSize);

        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;

    }
}

