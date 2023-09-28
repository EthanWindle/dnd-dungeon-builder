using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{
    public override bool canBeMovedByPlayer()
    {
        return true;
    }
}
