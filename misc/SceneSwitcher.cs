using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/**************************************************************
 * Switches between scenes
 **************************************************************/
public class SceneSwitcher : MonoBehaviour
{

    public static void switchToPodium()
    {
        SceneManager.LoadScene("Podium", LoadSceneMode.Single);
    }
    public static void switchToMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    public static void switchToRace()
    {
        SceneManager.LoadScene("Track", LoadSceneMode.Single);
    }
}
