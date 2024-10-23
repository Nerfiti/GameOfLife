using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settings_;

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenDeckEditor()
    {
        SceneManager.LoadScene("Deck Editor");
    }

    public void CloseSettings()
    {
        settings_.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
