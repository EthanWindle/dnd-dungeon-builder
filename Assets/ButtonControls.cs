using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public Animator flipPage;
    public GameObject text;
    
    //Home page Buttons
    public void GenerateNewMap(){
        StartCoroutine(generateMap());  
    }
    IEnumerator generateMap() {
        text.SetActive(false);
        flipPage.SetTrigger("LeftFlip");
        yield return new WaitForSeconds(1); 
        SceneManager.LoadScene("settingPage");
    }
    public void LoadMap(){
        StartCoroutine(load());
    }
    IEnumerator load() {
        text.SetActive(false);
        flipPage.SetTrigger("LeftFlip");
        yield return new WaitForSeconds(1); 
        SceneManager.LoadScene("loadPage");
    }

    public void Exit(){
        StartCoroutine(exit());
    }
    IEnumerator exit() {
        text.SetActive(false);
        flipPage.SetTrigger("Exit");
        yield return new WaitForSeconds(1); 
        Application.Quit();
    }


    //Settings page Buttons
    public void BackHome(){
        SceneManager.LoadScene("homePage");
    }

    public void Generate(){

    }

    public void ResetValues(){
        
    }
}
