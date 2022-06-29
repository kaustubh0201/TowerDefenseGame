using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private string levelToLoad = "LevelSelect";
    [SerializeField] private GameObject partToRotate;
    private float startAngle;
    private float endAngle;
    float speed = 0.6f;

    public void Play()
    {
        SceneManager.LoadScene(levelToLoad);
        Debug.Log("Play");
    }   

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    void Start(){
        startAngle = -70f;
        endAngle = -110f;

        
    }

    void Update(){
        
        float rY = Mathf.SmoothStep(startAngle, endAngle, Mathf.PingPong(Time.time * speed, 1));
        partToRotate.transform.rotation = Quaternion.Euler(0, rY, 0);

    }
}
