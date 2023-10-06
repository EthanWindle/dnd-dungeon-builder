using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * Controller for a door. Can be opened or closed. Can be entered when the door is open.
 */
public class DoorController : TileController
{

    private Sprite openSprite;
    private Sprite closeSprite;


    private void Awake()
    {
        _open = false;
    }



    public void Init(float size, SpritesheetManager spritesheetManager)
    {
        openSprite = spritesheetManager.Get("Open Door");
        closeSprite = spritesheetManager.Get("Closed Door");

        gameObject.GetComponent<SpriteRenderer>().sprite = closeSprite;

        base.Init(size);

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


    public void Switch(){
        if (this._open) Close();
        else Open();
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
