using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SettingsScript : MonoBehaviour
{
    public Button deleteAccountBTN;


    /**************************************************************
     * Start is called before the first frame update
     **************************************************************/
    void Start()
    {
        deleteAccountBTN.onClick.AddListener(deleteAccount);
    }


    /**************************************************************
     * Action of clicking on "delete account" -> "yes" button
     **************************************************************/
    public void deleteAccount()
    {
        StartCoroutine(DeleteAccountRequest(GameStats.loggedInUser.username));
        GameStats.clearAll();
    }


    /*************************************************************
     *  DeleteAccountRequest sends a request to the server, to 
     *  delete the given account (userName)
     **************************************************************/
    private IEnumerator DeleteAccountRequest(string userName)
    {
        string url = GlobalVariables.serverAddress + "/users?username=" + userName;

        UnityWebRequest request = UnityWebRequest.Delete(url);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Failed to delete account. Error: " + request.error);
        }
        else
        {
            Debug.Log("Account deleted successfully.");
        }
        request.downloadHandler.Dispose();
        request.Dispose();
    }
}
