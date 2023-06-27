using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class changePasswordScript : MonoBehaviour
{
    public Button savePasswordBTN;
    public TMPro.TMP_InputField oldPasswordInput;
    public TMPro.TMP_InputField newPasswordInput;
    public TMPro.TMP_Text errorMsg;


    /**************************************************************
     * Start is called before the first frame update
     **************************************************************/
    void Start()
    {
        savePasswordBTN.onClick.AddListener(savePassword);
    }


    /*************************************************************
     *  OnEnable function is called when the GameObject or 
     *  Component becomes active or enabled
     **************************************************************/
    void OnEnable()
    {
        oldPasswordInput.text = string.Empty;
        newPasswordInput.text = string.Empty;
        errorMsg.text = string.Empty;
    }


    /*************************************************************
     *  Action of clicking on the "save" button
     **************************************************************/
    public void savePassword()
    {
        string oldPassword = oldPasswordInput.text;
        string newPassword = newPasswordInput.text;

        if (string.IsNullOrEmpty(oldPassword))
        {
            errorMsg.text = "missing old password";
        }

        else if (string.IsNullOrEmpty(newPassword))
        {
            errorMsg.text = "missing new password";
        }

        else if (!SignUpScript.isValidPassword(newPassword))
        {
            errorMsg.text = "invalid new password";
        }

        else
        {
            StartCoroutine(changePasswordRequest(GameStats.loggedInUser.username, oldPassword, newPassword));
        }
    }


    /*************************************************************
     *  changePasswordRequest sends a request to the server, 
     *  to change the password of the given user from oldPassword 
     *  to newPassword
     **************************************************************/
    private IEnumerator changePasswordRequest(string username, string oldPassword, string newPassword)
    {
        string url = GlobalVariables.serverAddress + "/loginInfoCollection/password";

        // Create the JSON request body as a string
        string jsonRequestBody = "{\"username\":\"" + username + 
                                 "\",\"oldPassword\":\"" + oldPassword + 
                                 "\",\"newPassword\":\"" + newPassword + "\"}";

        // Create a UnityWebRequest instance with the PUT method and request body
        UnityWebRequest request = UnityWebRequest.Put(url, jsonRequestBody);
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request and wait for the response
        yield return request.SendWebRequest();

        // Check if the request encountered any errors
        if (request.result == UnityWebRequest.Result.Success)
        {
            // If there was no error - go back to the settings screen
            GameObject Settings = GameObject.Find("SettingsScreen");
            Settings.transform.GetChild(2).gameObject.SetActive(false);
            Settings.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            // If there was an error - show an error message
            errorMsg.text = "Error updating password";
        }
    }
}
