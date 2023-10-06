using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            load();
        }
        else 
        {
            load();
        }
    }

    /**
     * Changes the volume depending on the slider
     *
     */
    public void changeVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }

    /**
     * Responsible for loading the player preference of sound
     * last time they were in the executable
     *
     */
    private void load() 
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    /**
     * Responsible for saving the player preference of sound
     * for when they launch the executable
     *
     */
    private void save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }

}
