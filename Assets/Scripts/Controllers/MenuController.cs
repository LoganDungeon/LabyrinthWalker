using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewGame()
    {
        OptionsController.NewWorld = true;
        SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {
        OptionsController.NewWorld = false;
        SceneManager.LoadScene(1);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
