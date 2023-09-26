using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Controller for a Fog tile. Can always be entered.
 */
public class FogController : TileController
{
    public override bool CanEnter()
    {
        return false;
    }


}
