using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    //[SerializeField] private int frameCount;
    //[SerializeField ]private float frameRate;

   // private int currentFrame;
   // private float frameTimer;



    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorTexture, new Vector2(10,10), CursorMode.Auto);
    }

    private void Update()
    {
        /*   if (Input.GetMouseButtonDown(0) == true)
           {
               for (int i = 0; i < cursorTexture.Length; i++)
               {

                   Cursor.SetCursor(cursorTexture[i], new Vector2(10,10), CursorMode.Auto);
               }
           }
       }*/
    }

}
