using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class RoomController : MonoBehaviour
{

    public GameObject tilePrefab;
    public Shape[] shapes;

    private int maxWidth;
    private int maxHeight;


    void Awake()
    {
        maxWidth = 0;
        maxHeight = 0;
        foreach (Shape shape in shapes)
        {
            maxWidth = Mathf.Max(maxWidth, shape.x1);
            maxWidth = Mathf.Max(maxWidth, shape.x2);
            maxHeight = Mathf.Max(maxHeight, shape.y1);
            maxHeight = Mathf.Max(maxHeight, shape.y2);
        }

        //TODO: Also make sure that the shapes are not overlapping
    }


    public void PlaceTiles(Transform transformParent, GameObject[,] grid, float size)
    {
        Assert.IsTrue(grid.GetLength(0) >= maxWidth);
        Assert.IsTrue(grid.GetLength(1) >= maxHeight);

        

        foreach (Shape shape in shapes)
        {
            for (int x = shape.x1;  x < shape.x2; x++)
            {
                for (int y = shape.y1;  y < shape.y2; y++)
                {
                    GameObject tile = Instantiate(tilePrefab, new Vector3(x * size, y * size, 0), Quaternion.identity, transformParent);
                    tile.GetComponent<TileController>().Init(size);
                    grid[x, y] = tile;
                }
            }
        }
    }

    
}
