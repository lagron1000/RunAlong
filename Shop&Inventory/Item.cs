using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item")]
public class Item : ScriptableObject
{
    public string _id;
    public string name;
    public string type;
    public string description;
    public int price;
    public Sprite icon;
}
