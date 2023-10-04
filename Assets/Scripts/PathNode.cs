using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public GameObject[,] grid;
    public int x;
    public int y;
    public int gCost;
    public int hCost;
    public int fCost;
    GameObject fog;

    public PathNode previousNode;
    public PathNode(GameObject[,] grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void CreateFog(Transform transformParent, GameObject fogPrefab, GameObject[,] gridFogLayer, float size, float margin)
    {
        fog = UnityEngine.Object.Instantiate(fogPrefab, new Vector3((x) * (size + margin), (y) * (size + margin), -2), Quaternion.identity, transformParent);
        fog.GetComponent<TileController>().Init(size - margin * 2);
        gridFogLayer[x, y] = fog;
    }

    public void hideFogTile()
    {
        fog.SetActive(false);
    }

    public void showFogTile()
    {
        fog.SetActive(true);
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
    public override string ToString()
    {
        return x + "," + y;
    }
}