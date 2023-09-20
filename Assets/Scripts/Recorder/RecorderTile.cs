using System;

[System.Serializable]
public class RecorderTile
{
    public String type;
    public int x;
    public int y;

    public RecorderTile(String type, int x, int y)
    {
        this.type = type;
        this.x = x;
        this.y = y;
    }
}