using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levels;
    public GameObject settings;

    private void Start()
    {
        mainMenu.SetActive(true);
        levels.SetActive(false);
        settings.SetActive(false);
    }

    public void FromMainToLevels()
    {
        mainMenu.SetActive(false);
        levels.SetActive(true);
    }
    public void FromMainToSettings()
    {
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }

    public void FromLevelsToMain()
    {
        levels.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void FromSettingsToMain()
    {
        settings.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void GoToScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
