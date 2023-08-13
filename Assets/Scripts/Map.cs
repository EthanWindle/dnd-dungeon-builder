using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    // Public properties
    private Grid grid;
    public List<Room> roomList = new List<Room>();

    // Method to print a message about the object
    public Map(Grid grid)
    {
        this.grid = grid;
    }

    public void drawMap()
    {
        grid.drawGrid();
        foreach (var room in roomList)
        {
            room.draw();
        }
    }

    public void addRoom(Room room)
    {
        roomList.Add(room);
    }
}
