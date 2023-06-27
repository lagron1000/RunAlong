using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;



public class MatchManager : MonoBehaviour
{

    public TextMeshProUGUI rank;
    public float trackLen;


    /**************************************************************
     * Start is called before the first frame update
     **************************************************************/
    void Start()
    {
        trackLen = GameStats.trackLength;
    }

    /*************************************************************
     *  OnEnable function is called when the GameObject or 
     *  Component becomes active or enabled
     **************************************************************/
    void OnEnable()
    {
        rank.text = GameStats.loggedInUser.rank.ToString();
    }

    void StartRace()
    {
        //START TRACK SCENE
        SceneManager.LoadScene("Track", LoadSceneMode.Single);
    }

    void EndRace()
    {
        GameStats.trackLength = -1;
    }
}
