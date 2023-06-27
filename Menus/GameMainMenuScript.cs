using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMainMenuScript : MonoBehaviour
{

    public Button logoutBTN;

    /**************************************************************
     * Start is called before the first frame update
     **************************************************************/
    void Start()
    {
        logoutBTN.onClick.AddListener(logout);

    }

    /**************************************************************
     * Logout logic
     **************************************************************/
    public void logout()
    {
        // logout
        GameStats.clearAll();
    }

}
