using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

/*
 * Grid full of tiles
 * 
 */
public class Grid {

    private readonly int width;
    private readonly int height;
    private readonly float cellSize;
    private Tile[,] gridArray;

    public Grid(int width, int height, float cellSize) 
    {
        Debug.Log("Creating grid. W: " + width + " H: " + height + " CS: " + cellSize);
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new Tile[width, height];

        drawGrid();
    }

    /*
     * Draws a grid on screen, currently just in debug but should later draw on UI once implemented
     */
    public void drawGrid() 
    {
        Console.WriteLine("Drawing grid");
        for (int x = 0; x < width; x++) 
        {
            for (int y = 0; y < height; y++) 
            {
                // Can view the following using gizmos
                Debug.DrawLine(GetWorldLocation(x, y), GetWorldLocation(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldLocation(x, y), GetWorldLocation(x + 1, y), Color.white, 100f);
            }
        }
    }

    /*
     * Setters
     */
    public void addTile(int x, int y, Tile tile)
    {
        gridArray[x, y] = tile;
    }

    /*
     * Getters
     */
    public Tile getTile(int x, int y)
    {
        return gridArray[x, y];
    }

    /*
     * Helpers
     */
    private Vector2 GetWorldLocation(int x, int y)
    {
        return new Vector2(x, y) * cellSize;
    }

}
