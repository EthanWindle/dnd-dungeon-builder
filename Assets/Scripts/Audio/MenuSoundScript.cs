using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSoundScript : MonoBehaviour
{

    // Fields
    private static MenuSoundScript instance = null;

    // Fetches the instance of the script
    public static MenuSoundScript sound
    {
        get { return instance; }
    }

    private void Awake()
    {
        // if it isn't null, and is not the instance, destroy preexisting
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        if (SceneManager.GetActiveScene().ToString() != "gamePage")
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
