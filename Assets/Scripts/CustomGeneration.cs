using System;

public class CustomGeneration
{

    private int roomCount;
    private bool hasMonsters;
    private bool hasProps;

    public CustomGeneration(int roomCount, bool hasMonsters, bool hasProps)
    {
        this.roomCount = roomCount;
        this.hasMonsters = hasMonsters;
        this.hasProps = hasProps;
    }

    public int GetRoomCount() 
    { 
        return this.roomCount; 
    }
    
    public bool HasMonsters()
    {
        return this.hasMonsters;
    }

    public bool HasProps()
    {
        return this.hasProps;
    }

}