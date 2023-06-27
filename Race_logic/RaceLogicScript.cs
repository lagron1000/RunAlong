using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System;

public class RaceLogicScript : MonoBehaviour
{
    private int SPEED_BONUS = 10;
    private int WON_BONUS = 50;
    private int MIN_XP = 100;
    private int MAX_XP = 500;
    private int MAX_PRIZE_FACTOR = 10;
    private int MIN_PRIZE_FACTOR = 50;



    public float trackLength;
    public MeshFilter plane;
    public GameObject MyRunner;
    public GameObject MyRival;
    private bool won = false;
    private bool hasEndedRace = false;
    private Transform finishLine;
    private float finishPosition;
    private float timer = 0f;
    private TimeSpan elapsedTime;
    private float zBias;
    private float myPos;
    private float rivalPos;

    // countdown variables
    public Sprite spriteGetReady;
    public Sprite sprite3;
    public Sprite sprite2;
    public Sprite sprite1;
    public Sprite spriteGo;
    public SpriteRenderer spriteRenderer;

    /*************************************************************
     *  countdown and start the race
     **************************************************************/
    IEnumerator CountdownCoroutine()
    {
        spriteRenderer.sprite = spriteGetReady;
        yield return new WaitForSeconds(1f);

        spriteRenderer.sprite = sprite3;
        yield return new WaitForSeconds(1f);

        spriteRenderer.sprite = sprite2;
        yield return new WaitForSeconds(1f);

        spriteRenderer.sprite = sprite1;
        yield return new WaitForSeconds(1f);

        spriteRenderer.sprite = spriteGo;
        yield return new WaitForSeconds(1f);

        spriteRenderer.sprite = null;

        timer = 0.0f;
        StartRace();
    }


    /**************************************************************
     * Start is called before the first frame update
     **************************************************************/
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        trackLength = GameStats.trackLength > 0 ? GameStats.trackLength : 100;
        zBias = transform.position.z;
        finishLine = transform.Find("Finish");
        finishPosition = finishLine.position.z - zBias;
        StartCoroutine(CountdownCoroutine());
    }


    /**************************************************************
     * Start the race - use the runner's logic and the rival's logic
     **************************************************************/
    void StartRace()
    {
        this.MyRunner.GetComponent<PlayerController>().runner.switchRunning(true);
        this.MyRival.GetComponent<BotController>().runner.switchRunning(true);
    }


    /**************************************************************
     * Update is called once per frame
     **************************************************************/
    void Update()
    {
        // update timer
        timer += Time.deltaTime;

        myPos = MyRunner.transform.position.z - zBias;
        rivalPos = MyRival.transform.position.z - zBias;
        if (myPos >= finishPosition)
        {
            this.MyRunner.GetComponent<PlayerController>().runner.switchRunning(false);
            if (rivalPos < finishPosition)
            {
                won = true;
            }
            // Making sure this happens just once since Update() happens every frame.
            if (!hasEndedRace)
            {
                hasEndedRace = true;
                EndRaceCoRoutine();
            }
        }
        if (rivalPos >= finishPosition)
        {
            this.MyRival.GetComponent<BotController>().runner.switchRunning(false);
            if (myPos < finishPosition)
            {
                won = false;
            }
        }
    }


    /**************************************************************
     * Calculate the prize for the user (coins amount) 
     **************************************************************/
    private int calculatePrize(float ran)
    {
        int prize = 0;
        int max = (int)(trackLength / MAX_PRIZE_FACTOR);
        int min = (int)(trackLength / MIN_PRIZE_FACTOR);

        // Choose a random prize from the range
        prize += (UnityEngine.Random.Range(min, max));

        if (won)
        {
            // Bonus for winning:
            prize += WON_BONUS;
            // Bonus for speed:
            prize += (int)(SPEED_BONUS * (ran / timer));
        }

        // Have the prize be relative to how much of the track the player ran
        prize = (int)(prize * (ran / trackLength));

        return prize;
    }


    /**************************************************************
     * Calculate the xp
     **************************************************************/
    private int calculateXP(float ran)
    {
        int xp = 0;
        // Max value to draft is how many meters the player ran.
        int max = (int)ran;
        int min;
        if (won)
        {
            min = MIN_XP;
        }
        else
        {
            min = -1 * MIN_XP;
        }
        xp += (UnityEngine.Random.Range(min, max));

        // Normilizng the drafted xp number. max should be 500xp (level up is every 1000xp).
        // The more the player ran the higher the chance to get 500xp.
        xp = Math.Min(MAX_XP, xp);

        return xp;
    }


    public void EndRaceCoRoutine()
    {
        StartCoroutine(EndRace());
    }


    /**************************************************************
     * End race logic
     **************************************************************/
    private IEnumerator EndRace()
    {
        float ran = Math.Min(Math.Max(myPos * trackLength / finishPosition, 0), trackLength);
        
        // convert timer from seconds to hours:minutes:seconds format
        elapsedTime = TimeSpan.FromSeconds(timer);
        string timeString = elapsedTime.ToString(@"hh\:mm\:ss");

        // calculate achievements
        int prize = (int)(calculatePrize(ran));
        int experiencePoints = (int)calculateXP(ran);

        // save race data
        yield return StartCoroutine(endRacePostRequest(ran, prize, experiencePoints, timeString));

        Screen.orientation = ScreenOrientation.Portrait;

        // enter end race screen
        SceneSwitcher.switchToPodium();
    }


    /**************************************************************
     * endRacePostRequest sends a request to the server to save the 
     * data of the race
     * ran - the number of km that the user ran
     * prize - the amount of coins that the user earned
     * experiencePoints - the amount of xp that the user earned
     **************************************************************/
    private IEnumerator endRacePostRequest(float ran, int prize, int experiencePoints, string timeString)
    {
        string jsonBody = "{\"track_length\":\"" + trackLength +
                            "\",\"ran\":\"" + ran +
                            "\",\"runner_username\":\"" + GameStats.loggedInUser.username +
                            "\",\"time\":\"" + timeString +
                            "\",\"is_winner\":\"" + won +
                            "\",\"coins_earned\":\"" + prize +
                            "\",\"xp_earned\":\"" + experiencePoints + "\" }";

        byte[] jsonRequestBodyBytes = System.Text.Encoding.UTF8.GetBytes(jsonBody);

        // Create a UnityWebRequest to send the request to the server
        UnityWebRequest webRequest = UnityWebRequest.Post(GlobalVariables.serverAddress +
                                                         "/racesCollection", "application/json");
        webRequest.uploadHandler = new UploadHandlerRaw(jsonRequestBodyBytes);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for response
        yield return webRequest.SendWebRequest();

        // Handle the response:

        // if there was an error
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error saving race data.");
        }

        else
        {
            // update the coins, xp and the rank of the user
            yield return updateAchievements();
        }
        webRequest.downloadHandler.Dispose();
        webRequest.Dispose();
    }


    /**************************************************************
    * locally update the achievemtnts of the user 
    * (coins, xp and rank), according to the given respose of 
    * the server
    **************************************************************/
    private void saveAchievements(string serverResponse)
    {
        // save xp, rank and coins:
        Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(serverResponse);

        if (data.ContainsKey("xp") && data.ContainsKey("rank") && data.ContainsKey("coins"))
        {
            if (int.TryParse(data["xp"].ToString(), out int xpValue) &&
                int.TryParse(data["rank"].ToString(), out int rankValue) &&
                int.TryParse(data["coins"].ToString(), out int coinsValue))
            {
                // save the data
                GameStats.loggedInUser.xp = xpValue;
                GameStats.loggedInUser.rank = rankValue;
                GameStats.loggedInUser.coins = coinsValue;
            }
            else
            {
                Debug.LogError("Failed to parse integer values.");
            }
        }
    }


    /**************************************************************
     * updateAchievements sends a request to the server to get the 
     * achievemtnts of the user (coins, xp and rank), and locally 
     * updates them.
     **************************************************************/
    private IEnumerator updateAchievements()
    {
        // Create a new UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Get(GlobalVariables.serverAddress + "/achievements/" +
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
                // locally save the server's response (the achievements)
                saveAchievements(responseText);
            }
        }
        request.downloadHandler.Dispose();
        request.Dispose();
    }


}
