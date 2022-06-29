using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private static int levelNum = 1;
    private string retry = "LevelSelect";
    private string mainMenuScene = "MainMenu";
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;

    void Start()
    {
        levelNum += 1;
        Debug.Log(levelNum);

        retryButton.onClick.AddListener(() => LoadLevelSelect());
        mainMenuButton.onClick.AddListener(() => LoadMainMenuScene());
    }

    public void LoadMainMenuScene(){
        SceneManager.LoadScene(mainMenuScene);
    }

    public void LoadLevelSelect(){
        SceneManager.LoadScene(retry);
    }
}
