using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : EntityController
{
    public override bool canBeMovedByPlayer()
    {
        return false;
    }
}
