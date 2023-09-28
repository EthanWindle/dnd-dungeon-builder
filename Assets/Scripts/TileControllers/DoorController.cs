using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Controller for a door. Can be opened or closed. Can be entered when the door is open.
 */
public class DoorController : TileController
{

    public Texture2D openTexture;
    public Texture2D closeTexture;


    private void Awake()
    {
        _open = false;
        Sprite sprite = Sprite.Create(closeTexture, new Rect(0, 0, closeTexture.width, closeTexture.height), new Vector2(0.5f, 0.5f), textureSize);

        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    private bool _open;
    public override bool CanEnter()
    {
        return _open;
    }

    public void Open()
    {
        this._open = true;
        Sprite sprite = Sprite.Create(openTexture, new Rect(0, 0, openTexture.width, openTexture.height), new Vector2(0.5f, 0.5f), textureSize);

        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
    public void Close()
    {
        this._open=false;
        Sprite sprite = Sprite.Create(closeTexture, new Rect(0, 0, closeTexture.width, closeTexture.height), new Vector2(0.5f, 0.5f), textureSize);

        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    private RoomController Parent;

    public void SetParent(RoomController parent)
    {
        this.Parent = parent;
    }

    public RoomController GetParent()
    {
        return this.Parent;
    }


}
