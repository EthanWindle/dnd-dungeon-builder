using System;

[System.Serializable]
public class RecorderTile
{
    public String type;
    public int x;
    public int y;
    public int room;
    public String layer;

    /*
     * If this is a tile like a prop or monster which has different options,
     * this value represnets its position in the array of options, otherwise is set to -1.
     */
    public int option = -1;

    private void defaultInitialiser(String type, int x, int y, int room)
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

    public RecorderTile(String type, int x, int y, int room)
    {
        defaultInitialiser(type, x, y, room);
    }

    public RecorderTile(String type, int x, int y, int room, int option)
    {
        this.option = option;
        defaultInitialiser(type, x, y, room);
    }
}