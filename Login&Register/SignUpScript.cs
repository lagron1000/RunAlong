using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System.Text.RegularExpressions;


public class SignUpScript : MonoBehaviour
{
    public TMPro.TMP_InputField usernameInput;
    public TMPro.TMP_InputField nicknameInput;
    public TMPro.TMP_InputField passwordInput;
    public Button registerBTN;
    public TMPro.TMP_Text errorMsg;

    /**************************************************************
     * Start is called before the first frame update
     **************************************************************/
    void Start()
    {
        registerBTN.onClick.AddListener(register);
    }


    /*************************************************************
     *  OnEnable function is called when the GameObject or 
     *  Component becomes active or enabled
     **************************************************************/
    void OnEnable()
    {
        usernameInput.text = string.Empty;
        nicknameInput.text = string.Empty;
        passwordInput.text = string.Empty;
        errorMsg.text = string.Empty;
    }


    /**************************************************************
    *  Action of subbmiting the registration form
    **************************************************************/
    public void register()
    {
        string username = usernameInput.text;
        string nickname = nicknameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username))
        {
            errorMsg.text = "Missing username";
        }

        else if (string.IsNullOrEmpty(nickname))
        {
            errorMsg.text = "Missing nickname";
        }

        else if (string.IsNullOrEmpty(password))
        {
            errorMsg.text = "Missing password";
        }

        else if (!isValidPassword(password))
        {
            errorMsg.text = "Invalid password";
        }

        else
        {          
            StartCoroutine(addUserPostRequest(username, nickname, password));
        }    
    }


    /**************************************************************
    * Rerurn whether the given password is valid
    * (Minimum six characters, at least one letter and one number)
    **************************************************************/
    public static bool isValidPassword(string password)
    {
        // Minimum six characters, at least one letter and one number:
        Regex passwordRegex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$");
        return passwordRegex.IsMatch(password);
    }


    /**************************************************************
    * addUserPostRequest sends Registraion request to the server, 
    * and login if the request succeeds
    **************************************************************/
    private IEnumerator addUserPostRequest(string username, string nickname, string password)
    {
        string url = GlobalVariables.serverAddress + "/usersCollection";

        string jsonBody = "{\"username\":\"" + username +
                          "\",\"nickname\":\"" + nickname +
                          "\",\"password\":\"" + password + "\"}";

        // Create a new UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Post(url, "POST");

        // Set the request body
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        // Set the request content type
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            // Username is taken
            errorMsg.text = "Username is taken";
        }
        else
        {
            // login to the new user
            StartCoroutine(loginScript.loginPostRequest(username, password, "RegisterMenu"));
        }
        request.Dispose();
    }

}

