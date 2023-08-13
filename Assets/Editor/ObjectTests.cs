using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class Tests
{
    [Test]
    /* 
     * Currently just creates objects
     * Will draw grid on screen if you run test then click gizmos
     */
    public void test1()
    {
        var grid = new Grid(10, 10, 1);
        Assert.AreEqual(1, 1); // Does nothing just shows green test :)
    }

    [Test]
    public void testTileAdded()
    {
        var grid = new Grid(10, 10, 1);
        var tile = new Tile(new Vector2(2, 2));
        grid.addTile((int)(tile.getLocation().x), (int)(tile.getLocation().x), tile);
        Assert.AreEqual(grid.getTile(2, 2), tile);
    }
}
