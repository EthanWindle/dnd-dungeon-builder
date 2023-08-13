using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A room will take up multiple tiles. Current thoughts are: put the rooms location in the top left tile then set the rest as null.
 * Rooms draw method will draw all the rooms.
 */
public class Room : Tile
{
    public Room(Vector2 location) : base(location)
    {

    }

    public void draw()
    {
        
    }
}
