using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Tile is the base class for rooms, paths, etc
 *  
 */
public class Tile
{
    private readonly Vector2 location;

    public Tile(Vector2 location)
    {
        this.location = location;
    }

    public Vector2 getLocation()
    {
        return location;
    }
}
