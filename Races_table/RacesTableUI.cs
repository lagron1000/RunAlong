using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using System;

public class RacesTableUI : MonoBehaviour
{
    public RowUI rowUI;
    private RaceStats[] races;
    public Transform racesContainer;


    /*************************************************************
     *  OnEnable function is called when the GameObject or 
     *  Component becomes active or enabled
     **************************************************************/
    void OnEnable()
    {
        StartCoroutine(GetRacesRequest());
    }


    /*************************************************************
     *  View the races of the logged-in user
     **************************************************************/
    private void PopulateTable()
    {
        foreach (Transform child in racesContainer)
        {
            Destroy(child.gameObject);
        }

        int racesNum = races.Length;
        for (int i = (racesNum - 1); i >= 0; i--)
        {
            int index = i;
            var row = Instantiate(rowUI, transform).GetComponent<RowUI>();
            row.date.text = races[index].date;
            float ran_km = (races[index].ran / 1000f);
            float length_km =(races[index].track_length / 1000f);
            row.ran.text = String.Format("{0:0.00}", ran_km) + " of " + String.Format("{0:0.00}", length_km);
            row.time.text = races[index].time;
            if (races[index].is_winner)
            {
                row.reward.enabled = true;
        }
            else
            {
                row.reward.enabled = false;
            }
        }
    }


    /*************************************************************
     *  GetRacesRequest sends request to the server, to get
     *  the races of the logged-in user, and populates the 
     *  races table if it succeeds. 
     **************************************************************/
    private IEnumerator GetRacesRequest()
    {
        string url = GlobalVariables.serverAddress + "/racesCollection/" + GameStats.loggedInUser.username;
        // Create a new UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Get(url);

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Request error: " + request.error);
        }
        else
        {
            // Get the response text
            string responseText = request.downloadHandler.text;
            if (Requests.IsSuccess(request))
            {
                this.races = JsonConvert.DeserializeObject<RaceStats[]>(responseText);
                PopulateTable();
            }
        }
        request.downloadHandler.Dispose();
        request.Dispose();
    }
}
