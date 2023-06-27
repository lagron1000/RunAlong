using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;


public class Requests : MonoBehaviour
{
    /*************************************************************
     *  Return whether the request succeeded
     **************************************************************/
    public static bool IsSuccess(UnityWebRequest request)
    {
        if (request.result == UnityWebRequest.Result.ConnectionError) { return false; }
        if (request.responseCode == 0) { return true; }
        if (request.responseCode == (long)System.Net.HttpStatusCode.OK) { return true; }

        return false;
    }

    /*************************************************************
     *  Post request to the server
     **************************************************************/
    public static IEnumerator PostRequest(string url, string jsonBody)
    {
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
            Debug.LogError("Request error: " + request.error);
        }
        request.downloadHandler.Dispose();
        request.Dispose();
    }


    /*************************************************************
     *  Put request to the server
     **************************************************************/
    public static IEnumerator PutRequest(string url, string jsonData)
    {
        // Create a UnityWebRequest object with the desired URL and PUT method
        byte[] jsonRequestBodyBytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
        using (UnityWebRequest request = UnityWebRequest.Put(url, jsonRequestBodyBytes))
        {
            // Set the content type to "application/json"
            request.SetRequestHeader("Content-Type", "application/json");

            // Send the request
            yield return request.SendWebRequest();

            // Check for errors
            if (request.result == UnityWebRequest.Result.ConnectionError || 
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("PUT request failed: " + request.error);
            }
            request.downloadHandler.Dispose();
            request.Dispose();

        }
    }
}
