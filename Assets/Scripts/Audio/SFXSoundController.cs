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
}
