using System;

[System.Serializable]
public class RecorderRoom
{
    public int RoomNumber;
    public int XPos;
    public int YPos;

    public RecorderRoom(int roomNumber, int x, int y)
    {
        this.RoomNumber = roomNumber;
        this.XPos = x;
        this.YPos = y;
    }
}