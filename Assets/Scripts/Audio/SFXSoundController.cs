using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXSoundController : MonoBehaviour
{

    public AudioSource audio;

    public void playSoundEffect() 
    { 
        audio.Play();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) == true)
        {
            audio.Play();
        }
    }
}
