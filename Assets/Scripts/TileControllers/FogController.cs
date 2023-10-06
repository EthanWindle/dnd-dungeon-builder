using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Controller for a Fog tile. Can always be entered.
 */
public class FogController : TileController
{
    public Texture2D highOpacityTile;
    public Texture2D lowOpacityTile;

    private int textureSize = 16;

    private void Awake()
    {
        Sprite sprite = Sprite.Create(highOpacityTile, new Rect(0, 0, highOpacityTile.width, highOpacityTile.height), new Vector2(0.5f, 0.5f), textureSize);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public override bool CanEnter()
    {
        return false;
    }

    public void lowerOpacity()
    {
        Sprite sprite = Sprite.Create(lowOpacityTile, new Rect(0, 0, lowOpacityTile.width, lowOpacityTile.height), new Vector2(0.5f, 0.5f), textureSize);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void raiseOpacity()
    {
        Sprite sprite = Sprite.Create(highOpacityTile, new Rect(0, 0, highOpacityTile.width, highOpacityTile.height), new Vector2(0.5f, 0.5f), textureSize);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

}
