using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelWon : MonoBehaviour
{
    private string levelCompleteScene = "LevelSelect";
    private string mainMenuScene = "MainMenu";
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button mainMenuButton;

    void Start(){
        
        nextLevelButton.onClick.AddListener(() => LoadAnotherScene());
        mainMenuButton.onClick.AddListener(() => LoadMainMenuScene());
    }

    public void LoadMainMenuScene(){
        SceneManager.LoadScene(mainMenuScene);
    }

    public void LoadAnotherScene(){
        SceneManager.LoadScene(levelCompleteScene);
    }
}
