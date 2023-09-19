using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class NewBehaviourScript : MonoBehaviour
{
    public Animator flipPage;
    public GameObject mainText;

    public GameObject savePrefab;
    public Transform saveParent;
    public TMP_Text map;
    public void toMain(){
        SceneManager.LoadScene("homePage");
    }
    
    //Home page Buttons
    public void GenerateNewMap(){
        StartCoroutine(generateMap());  
    }
    IEnumerator generateMap() {
        mainText.SetActive(false);
        flipPage.SetTrigger("LeftFlip");
        yield return new WaitForSeconds(1); 
        SceneManager.LoadScene("settingPage");
    }
    public void LoadMap(){
        StartCoroutine(load());
    }
    IEnumerator load() {
        mainText.SetActive(false);
        flipPage.SetTrigger("LeftFlip");
        yield return new WaitForSeconds(1); 
        SceneManager.LoadScene("loadPage");
    }

    public void ExitGame(){
        StartCoroutine(exit());
    }
    IEnumerator exit() {
        mainText.SetActive(false);
        flipPage.SetTrigger("Exit");
        yield return new WaitForSeconds(1); 
        Application.Quit();
    }


    //Settings page Buttons
    public void BackHome(){
        StartCoroutine(back());
    }
    IEnumerator back() {
        mainText.SetActive(false);
        flipPage.SetTrigger("RightFlip");
        yield return new WaitForSeconds(2); 
        SceneManager.LoadScene("homePage");
    }

    public void Generate(){

    }


    //load page 
    public void SetSavedGame(string name, TMP_Text mapText){
        mapText.text = name;
    }
    public void GetSavedGames() {
        
        DirectoryInfo dir = new DirectoryInfo("Assets/Saves");
        FileInfo[] files = dir.GetFiles("*.json");

        foreach(FileInfo file in files){
            string fileName = Path.GetFileNameWithoutExtension(file.Name); 
            GameObject newButton = Instantiate(savePrefab, saveParent);
            TMP_Text[] texts = newButton.GetComponentsInChildren<TMP_Text>();
            texts[0].text = fileName;
            newButton.GetComponent<Button>().onClick.AddListener(() => SetSavedGame(fileName, map));
        }
    }

    public void loadGame(TMP_Text map){
        if(Equals(map.text, "Map name")){
            print("No File selected");
        } else {
            //call Adams Load Method
            print("Load");
        }
    }

}
