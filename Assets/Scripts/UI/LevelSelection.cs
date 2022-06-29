using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public void GoToLevel(int level){
        SceneManager.LoadScene("Level_" + level);
    }
}
