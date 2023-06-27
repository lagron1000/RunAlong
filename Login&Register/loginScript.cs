using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;



public class loginScript : MonoBehaviour
{
    public TMPro.TMP_InputField usernameInput;
    public TMPro.TMP_InputField passwordInput;
    public Button loginBTN;
    public TMPro.TMP_Text errorMsg;


    /**************************************************************
     * Start is called before the first frame update
     **************************************************************/
    void Start()
    {
        loginBTN.onClick.AddListener(login);
        if (GameStats.loggedInUser != null)
        {
            GameObject.Find("LoginMenu").gameObject.SetActive(false);
            GameObject.Find("GameMainMenu").transform.GetChild(1).gameObject.SetActive(true);

        }
    }


    /**************************************************************
     *  Action of subbmiting the login form
     **************************************************************/
    public void login()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username))
        {
            errorMsg.text = "Missing username";
        }

        else if (string.IsNullOrEmpty(password))
        {
            errorMsg.text = "Missing password";
        }

        else
        {
            StartCoroutine(loginPostRequest(username, password, "LoginMenu"));
        }
    }


    /*************************************************************
     *  OnEnable function is called when the GameObject or 
     *  Component becomes active or enabled
     **************************************************************/
    void OnEnable()
    {
        usernameInput.text = string.Empty;
        passwordInput.text = string.Empty;
        errorMsg.text = string.Empty;
    }


    /**************************************************************
    * loginPostRequest sends login request to the server
    **************************************************************/
    public static IEnumerator loginPostRequest(string username, string password, string screen)
    {
        string jsonBody = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}";
        byte[] jsonRequestBodyBytes = System.Text.Encoding.UTF8.GetBytes(jsonBody);

        // Create a UnityWebRequest to send the login request to the server
        UnityWebRequest webRequest = UnityWebRequest.Post(GlobalVariables.serverAddress + 
                                                          "/login", "application/json");

        webRequest.uploadHandler = new UploadHandlerRaw(jsonRequestBodyBytes);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for response
        yield return webRequest.SendWebRequest();

        // Handle the response:

        // login error
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            if(screen == "LoginMenu")
            {
                GameObject loginErrorMsg = GameObject.Find("LoginErrorMsg");
                TMPro.TMP_Text error = loginErrorMsg.GetComponent<TMP_Text>();
                if (error != null)
                {
                    error.text = "Invalid username or password";
                }
            }
        }

        // successful login
        else
        {
            string responseText = webRequest.downloadHandler.text;
            if (responseText != null)
            {
                // Get the response text
                GameStats.loggedInUser = JsonUtility.FromJson<GameStats>(responseText);
                GameObject.Find(screen).SetActive(false);
                GameObject GameMainMenu = GameObject.Find("GameMainMenu");
                GameMainMenu.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("Login failed. Invalid response from server.");
            }
        }
        webRequest.downloadHandler.Dispose();
        webRequest.Dispose();
    }

}
