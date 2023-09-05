using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Controller for a Floor tile. Can always be entered.
 */
public class FloorController : TileController
{
    public override bool CanEnter()
    {
        return true;
    }


}
