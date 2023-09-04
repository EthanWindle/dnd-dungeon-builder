using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Controller for a door. Can be opened or closed. Can be entered when the door is open.
 */
public class DoorController : TileController
{

    public Sprite openSprite;
    public Sprite closeSprite;


    private void Awake()
    {
        _open = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = closeSprite;
    }

    private bool _open;
    public override bool CanEnter()
    {
        return _open;
    }

    public void Open()
    {
        this._open = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = openSprite;
    }
    public void Close()
    {
        this._open=false;
        gameObject.GetComponent<SpriteRenderer>().sprite = closeSprite;
    }

}
