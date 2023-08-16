using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    //Home page Buttons
    public void GenerateNewMap(){
        SceneManager.LoadScene("settingPage");
    }


    //Settings page Buttons
    public void BackHome(){
        SceneManager.LoadScene("homePage");
    }
}
