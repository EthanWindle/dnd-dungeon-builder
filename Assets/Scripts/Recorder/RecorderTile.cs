using System;

[System.Serializable]
public class RecorderTile
{
    public String type;
    public int x;
    public int y;
    public int room;
    public String layer;

    public RecorderTile(String type, int x, int y, int room)
    {
        this.type = type;
        this.x = x;
        this.y = y;
        this.room = room;
        if (type.Equals("floor") || type.Equals("door") || type.Equals("wall"))
        {
            this.layer = "background";
        }
        else if (type.Equals("fog"))
        {
            this.layer = "fogLayer";
        }
        else
        {
            this.layer = "foreground";
        }
    }
}