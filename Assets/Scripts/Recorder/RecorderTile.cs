using System;
using System.Diagnostics;

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
    public String option;

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

    public RecorderTile(String type, int x, int y, int room, String option)
    {
        this.option = RemoveLastWord(option);
        defaultInitialiser(type, x, y, room);
    }

    private static string RemoveLastWord(string input)
    {
        // Split the input string into words
        string[] words = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        // Check if there are more than one word in the input string
        if (words.Length > 1)
        {
            // Remove the last word by creating a new string with all words except the last one
            return string.Join(" ", words, 0, words.Length - 1);
        }
        else
        {
            // If there is only one word, return it as is
            return input;
        }
    }
}