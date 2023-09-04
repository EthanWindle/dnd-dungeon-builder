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
}

