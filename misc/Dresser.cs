using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dresser : MonoBehaviour
{

    public GameObject character;
    public bool human;
    private string BOT_TOP_MATERIAL = "6454d77301ba82fa1931ea51";
    private string BOT_BOTTOM_MATERIAL = "6454d7f201ba82fa1931ea55";
    private string DEFAULT_SHOES_MAT = "BrownFabric";

    // Start is called before the first frame update
    void Start()
    {
        if (human)
        {
            dressPlayer();
        }
        else
        {
            dressBot();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /**************************************************************
     * Dressing player based on clothes he has on according to the server
     **************************************************************/
    private void dressPlayer()
    {
        if (GameStats.loggedInUser != null)
        {
            wearShirt(ShopItemsCollection.GetMaterial(GameStats.loggedInUser.top));
            wearPants(ShopItemsCollection.GetMaterial(GameStats.loggedInUser.bottom));
            wearShoes(ShopItemsCollection.GetMaterial(GameStats.loggedInUser.shoes));
            paintNails((ShopItemsCollection.getRandMaterial()));
        }
        else
        {
            dressBot();
        }
    }


    /**************************************************************
     * Dressing the bot in the clothes we chose for him
     **************************************************************/
    private void dressBot()
    {
        wearShirt(ShopItemsCollection.GetMaterial(BOT_TOP_MATERIAL));
        wearPants(ShopItemsCollection.GetMaterial(BOT_BOTTOM_MATERIAL));
        paintNails((ShopItemsCollection.getRandMaterial()));
        wearShoes(getMaterial(DEFAULT_SHOES_MAT));
    }


    /**************************************************************
     * dresses the model in random clothes
     **************************************************************/
    private void dressRandomly()
    {
        wearShirt(ShopItemsCollection.getRandMaterial());
        wearPants(ShopItemsCollection.getRandMaterial());
        paintNails((ShopItemsCollection.getRandMaterial()));
        wearShoes(ShopItemsCollection.getRandMaterial());
    }

    /**************************************************************
     * Changing the shirt
     **************************************************************/
    private void wearShirt(Material shirt)
    {
        character.transform.Find("Short shirt").GetComponent<Renderer>().material = shirt;
    }


    /**************************************************************
     * Changing the pants
     **************************************************************/
    private void wearPants(Material pants)
    {
        character.transform.Find("Short pants").GetComponent<Renderer>().material = pants;
    }


    /**************************************************************
     * Changing the shoes
     **************************************************************/
    private void wearShoes(Material shoes)
    {
        character.transform.Find("Sandles").GetComponent<Renderer>().material = shoes;
    }


    /**************************************************************
     * Changing the nails color
     **************************************************************/
    private void paintNails(Material color)
    {
        Material[] matArray = character.transform.Find("Body").GetComponent<Renderer>().materials;
        matArray[1] = color;
        character.transform.Find("Body").GetComponent<Renderer>().materials = matArray;
    }

    private Material getMaterial(string name)
    {
        return Resources.Load("Models/Clothes/"+name, typeof(Material)) as Material;
    }
}
