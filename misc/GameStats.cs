using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats
{
    // Static variable to store the logged-in user info
    public static GameStats loggedInUser;

    // Logged-in user info
    public string _id;
    public string username;
    public string nickname;
    public int rank;
    public int coins;
    public string top;
    public string bottom;
    public string shoes;
    public List<string> inventory;
    public int xp;
    public static float trackLength = 10;

    public GameStats()
    {
        _id = string.Empty;
        username = string.Empty;
        nickname = string.Empty;
        rank = 0;
        coins = 0;
        top = string.Empty;
        bottom = string.Empty;
        shoes = string.Empty;
        inventory = new List<string>();
        xp = 0;
    }

    public static void clearAll()
    {
        loggedInUser._id = string.Empty;
        loggedInUser.username = string.Empty;
        loggedInUser.nickname = string.Empty;
        loggedInUser.rank = 0;
        loggedInUser.coins = 0;
        loggedInUser.top = string.Empty;
        loggedInUser.bottom = string.Empty;
        loggedInUser.shoes = string.Empty;
        loggedInUser.inventory = new List<string>();
        loggedInUser.xp = 0;
    }

    private void Awake()
    {
        loggedInUser = this;
    }
}
