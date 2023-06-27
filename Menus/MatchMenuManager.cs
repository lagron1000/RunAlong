using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MatchMenuManager : MonoBehaviour
{
    public int runLength;


    /**************************************************************
     * Handles race lenmgth input
     **************************************************************/
    public void HandleInputDate(int input)
    {
        switch(input) {
            case 0 : runLength = -1;
                break;
            case 1: runLength = 50;
                break;
            case 2:
                runLength = 1000;
                break;
            case 3:
                runLength = 5000;
                break;
            case 4:
                runLength = 10000;
                break;
        }
    }

    public void HandleStartRace()
    {
        if (runLength > 0)
        {
            GameStats.trackLength = runLength;
            SceneSwitcher.switchToRace();
        }
        else
        {
            Debug.Log("Please choose a track length");
        }
    }
}
