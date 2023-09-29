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

    private void Awake()
    {
        Sprite sprite = Sprite.Create(highOpacity, new Rect(0, 0, highOpacity.width, highOpacity.height), new Vector2(0.5f, 0.5f), textureSize);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public override bool CanEnter()
    {
        return false;
    }

    public void lowerOpacity()
    {
        Sprite sprite = Sprite.Create(lowOpacity, new Rect(0, 0, lowOpacity.width, lowOpacity.height), new Vector2(0.5f, 0.5f), textureSize);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void raiseOpacity()
    {
        Sprite sprite = Sprite.Create(highOpacity, new Rect(0, 0, highOpacity.width, highOpacity.height), new Vector2(0.5f, 0.5f), textureSize);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

}
