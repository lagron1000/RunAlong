using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class podiumLogic : MonoBehaviour
{
    public GameObject Player;
    public GameObject Bot;
    private Transform first;
    private Transform second;
    private Transform third;
    public Animator playerAnimator;
    public Animator botAnimator;
    public TextMeshProUGUI rank;



    /**************************************************************
     * Start is called before the first frame update
     **************************************************************/
    IEnumerator Start()
    {
        // Create a new UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Get(GlobalVariables.serverAddress + 
                                                      "/lastRace/" + 
                                                      GameStats.loggedInUser.username);

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
                RaceStats stats = JsonConvert.DeserializeObject<RaceStats>(responseText);
                bool won = stats.is_winner;
                int coins = stats.coins_earned;
                startPodium(won, coins);
            }
        }
        request.Dispose();
    }


    /**************************************************************
     * Set the podium settings
     **************************************************************/
    private void startPodium(bool won, int coins)
    {
        // We have three empty gameObjects that are saved to grab their position in space.
        first = GameObject.Find("1st").transform;
        second = GameObject.Find("2nd").transform;
        third = GameObject.Find("3rd").transform;
        GameObject.Find("CoinsCounter").GetComponent<TMPro.TextMeshProUGUI>().text = coins.ToString();
        rank.text = GameStats.loggedInUser.rank.ToString();

        if (won)
        {
            Player.transform.position = first.position;
            Bot.transform.position = new Vector3(second.position.x, -10.73f, second.position.z);
            GameObject.Find("LossingText").SetActive(false);
            playerAnimator.SetBool("lost", false);
            botAnimator.SetBool("lost", true);
            GameObject.Find("victorySound").GetComponent<AudioSource>().Play(64100);
        }

        else
        {
            Bot.transform.position = first.position;
            Player.transform.position = new Vector3(second.position.x, -10.73f, second.position.z);
            GameObject.Find("VictoryText").SetActive(false);
            playerAnimator.SetBool("lost", true);
            botAnimator.SetBool("lost", false);
            GameObject.Find("loosingSound").GetComponent<AudioSource>().Play(64100);

        }

        return;
    }

}
