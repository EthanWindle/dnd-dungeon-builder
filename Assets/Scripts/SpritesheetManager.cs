using System.Collections;
using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.U2D;

public class SpritesheetManager
{

    /**
     * This is a really hacky implementation.
     * Specific sprites need to be in the following indices:
     * 0: Wall_Left
     * 1: Wall_Right
     * 2: Wall_Top
     * 3: Wall_Bottom
     * 4: Wall_Top_Left
     * 5: Wall_Top_Right
     * 6: Wall_Bottom_Left
     * 7: Wall_Bottom_Right
     * 8: Wall_Small_Top_Left
     * 9: Wall_Small_Top_Right
     * 10:Wall_Small_Bottom_Left
     * 11:Wall_Small_Bottom_Right
     * 12:Floor_Main
     * 13:Floor_1
     * 14:Floor_2
     * 15:Floor_3
     * 16:Floor_4
     * 17:Door
     * 18:Open_Door
     */

    private readonly Dictionary<string, Sprite> textures;
   public SpritesheetManager(string spritesheetName)
    {

        textures = new Dictionary<string, Sprite>();

        Sprite[] sprites = Resources.LoadAll<Sprite>(spritesheetName);

        foreach (Sprite sprite in sprites)
        {
            textures[sprite.name] = sprite;
        }

    }

    public Sprite Get(string textureName)
    {
        return textures[textureName];
    }

}
