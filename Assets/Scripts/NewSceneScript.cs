using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewSceneScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToGameScene() {
        SceneManager.LoadScene("ComputerScreen");
    }

    public void GoToHelpScene() {
        SceneManager.LoadScene("HelpScreen");
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene("Main Menu");
    }

    public void Quit() {
        Application.Quit();
    }
}
