using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("DungeonTest");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
