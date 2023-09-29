using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Controller for a Fog tile. Can always be entered.
 */
public class FogController : TileController
{
    public Texture2D highOpacity;
    public Texture2D lowOpacity;
    //public SpriteRenderer spriteRenderer;
    //public Sprite highOpacity;
    //public Sprite lowOpacity;

    /*
     * GameObject wall = Instantiate(wallTile, new Vector3(x * (cellSize + cellSpacing), y * (cellSize + cellSpacing), 1), Quaternion.identity, gameObject.transform);
            wall.GetComponent<WallController>().Init(cellSize - cellSpacing * 2);
            wall.GetComponent<WallController>().SetTexture(GetAdjacentControllers(x, y));
     */

    private void Awake()
    {
        Sprite sprite = Sprite.Create(highOpacity, new Rect(0, 0, highOpacity.width, highOpacity.height), new Vector2(0.5f, 0.5f), textureSize);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        //spriteRenderer.sprite = highOpacity;
    }

    public override bool CanEnter()
    {
        return false;
    }

    public void lowerOpacity()
    {
        Sprite sprite = Sprite.Create(lowOpacity, new Rect(0, 0, lowOpacity.width, lowOpacity.height), new Vector2(0.5f, 0.5f), textureSize);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        //spriteRenderer.sprite = lowOpacity;

    }

    public void raiseOpacity()
    {
        Sprite sprite = Sprite.Create(highOpacity, new Rect(0, 0, highOpacity.width, highOpacity.height), new Vector2(0.5f, 0.5f), textureSize);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        //spriteRenderer.sprite = lowOpacity;

    }






}
