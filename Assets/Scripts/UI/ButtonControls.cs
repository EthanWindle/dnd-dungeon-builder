using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class ButtonControls : MonoBehaviour
{
    public Animator flipPage;
    public GameObject mainText;

    public GameObject savePrefab;
    public Transform saveParent;
    public TMP_Text map;

    public GameObject Grid;
    public GameObject Override;

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

    // Multiplayer Page
    public void loadMultiplayer() {
        StartCoroutine(multiplayerPage());
    }
    IEnumerator multiplayerPage() {
        mainText.SetActive(false);
        flipPage.SetTrigger("LeftFlip");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("multiplayerPage");
    }

    // Credits Page
    public void loadCredits() {
        StartCoroutine(creditsPage());
    }

    IEnumerator creditsPage() {
        mainText.SetActive(false);
        flipPage.SetTrigger("LeftFlip");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("creditsPage");
    }

    // Credits Page
    public void loadOptions()
    {
        StartCoroutine(optionsPage());
    }

    IEnumerator optionsPage()
    {
        mainText.SetActive(false);
        flipPage.SetTrigger("LeftFlip");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("optionsPage");
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
        SceneManager.LoadScene("mapScene");
    }


    //load page 
    public void SetSavedGame(string name, TMP_Text mapText){
        mapText.text = name;
    }
    public void GetSavedGames() {
        
        DirectoryInfo dir = new DirectoryInfo("Assets/Saves");
        FileInfo[] files = dir.GetFiles("*.json");

        foreach(FileInfo file in files){
            string fileName = System.IO.Path.GetFileNameWithoutExtension(file.Name); 
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
            string str = map.text;
            GlobalVariables.setMapName(str);
            SceneManager.LoadScene("MapScene");
        }
    }

    public void ReturnToSettings(){
        StartCoroutine(returnToSettings());
    }

    IEnumerator returnToSettings() {
        mainText.SetActive(false);
        flipPage.SetTrigger("Close");
        yield return new WaitForSeconds(2); 
        SceneManager.LoadScene("settingPage");
    }

    public void loadContent(){
        mainText.SetActive(true);
    }

    //mapScene
    public void SetSavedMap(string name, TMP_InputField mapName){
        mapName.text = name;
    }
    public void GetSavedMaps(TMP_InputField mapName) {
        DirectoryInfo dir = new DirectoryInfo("Assets/Saves");
        FileInfo[] files = dir.GetFiles("*.json");
        foreach(FileInfo file in files){
            string fileName = System.IO.Path.GetFileNameWithoutExtension(file.Name); 
            GameObject newButton = Instantiate(savePrefab, saveParent);
            TMP_Text[] texts = newButton.GetComponentsInChildren<TMP_Text>();
            texts[0].text = fileName;
            newButton.GetComponent<Button>().onClick.AddListener(() => SetSavedMap(fileName, mapName));
        }
    }

     public void saveMap(TMP_InputField name){
        if(!string.IsNullOrWhiteSpace(name.text)){
            //Check if exsiting file is going to be overwriten
            DirectoryInfo dir = new DirectoryInfo("Assets/Saves");
            FileInfo[] files = dir.GetFiles("*.json");
            bool newFile = true;
            foreach(FileInfo file in files){
                string fileName = System.IO.Path.GetFileNameWithoutExtension(file.Name); 
                if(Equals(fileName, name.text)){
                    newFile = false;
                    break;
                }
            }

            if(newFile){
                saveFile(name);
            } else {
                Override.SetActive(true);
            }
        }
    }

    public void RemoveMonsterControllers()
    {
        // Find all game objects with the "Monster" tag
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        //Debug.Log("Array Count: " + monsters.Length);
        foreach (GameObject monster in monsters)
        {
            // Remove the MonsterMovementController component immediately
            MonsterMovementController controller = monster.GetComponent<MonsterMovementController>();
            if (controller != null)
            {
                DestroyImmediate(controller);
            }
        }
    }

    public void AddMonsterMovementControllers()
    {
        // Find all game objects with the "Monster" tag
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monsters)
        {
            // Check if the MonsterMovementController component doesn't already exist
            if (monster.GetComponent<MonsterMovementController>() == null)
            {
                // Add the MonsterMovementController component
                monster.AddComponent<MonsterMovementController>();
            }
        }
    }

    public void saveFile(TMP_InputField name){
        string fullFileName = "Assets/Saves/" + name.text + ".json";
        Grid.GetComponent<GridController>().Save(fullFileName);
    }
}
