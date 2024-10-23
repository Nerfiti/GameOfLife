using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pause_menu_;
    
    public void OpenPauseMenu()
    {
        pause_menu_.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        pause_menu_.SetActive(false);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main menu");    
    }
}
