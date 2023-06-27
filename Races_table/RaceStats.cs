using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RaceStats")]
public class RaceStats : ScriptableObject
{
    public string _id;
    public int track_length;
    public float ran;
    public string runner_username;
    public string time;
    public bool is_winner;
    public int coins_earned;
    public int xp_earned;
    public string date;

}
