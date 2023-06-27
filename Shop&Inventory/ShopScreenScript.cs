using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;


public class ShopScreenScript : MonoBehaviour
{
    public TextMeshProUGUI coinsAmount;
    public GameObject shopItemPrefab;
    public Transform itemsContainer;
    private Item[] items;
    public GameObject buyItemBTN;
    public GameObject character;
    public GameObject buyErrorMsg;


    /**************************************************************
     * Start is called before the first frame update
     **************************************************************/
    void Start()
    {
        StartCoroutine(shopGetRequest());
        Button buttonComponent = buyItemBTN.AddComponent<Button>();
        buyItemBTN.SetActive(false);
    }


    /*************************************************************
     *  OnEnable function is called when the GameObject or 
     *  Component becomes active or enabled
     **************************************************************/
    void OnEnable()
    {
        buyItemBTN.SetActive(false);
        buyErrorMsg.SetActive(false);
        coinsAmount.text = GameStats.loggedInUser.coins.ToString();
        ShopAndInventory.wearCurrOutfit(character);
    }


    /*************************************************************
     *  View the items of the shop
     **************************************************************/
    private void PopulateShop()
    {
        int len = items.Length;
        for (int i = 0; i < len; i++)
        {
            int index = i;
            GameObject obj = Instantiate(shopItemPrefab, itemsContainer) as GameObject;
            // Button
            obj.GetComponent<Button>().onClick.AddListener(() => OnItemClick(index));
            // Name
            obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = items[index].name;
            // Icon
            obj.transform.GetChild(1).GetComponent<Image>().sprite = ShopItemsCollection.GetImage(items[index]._id);
            // Price
            obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = items[index].price.ToString();
        }
    }


    /*************************************************************
     *  Action of clicking on a shop item
     **************************************************************/
    private void OnItemClick(int itemIndex)
    {
        // hide error message
        buyErrorMsg.SetActive(false);

        Item item = items[itemIndex];

        // Dress up the character
        ShopAndInventory.DressUp(item.type, item._id, character);

        // if the user already has this item, then the "Buy" button shouldn't appear:
        if (GameStats.loggedInUser.inventory.Contains(items[itemIndex]._id))
        {
            buyItemBTN.SetActive(false);
        }
        // otherwise - show the "Buy" Button
        else
        {
            buyItemBTN.SetActive(true);
            Button buttonComponent = buyItemBTN.GetComponent<Button>();
            buttonComponent.onClick.RemoveAllListeners();
            buttonComponent.onClick.AddListener(() => buyItem(itemIndex));
        }
    }


    /*************************************************************
     *  Action of clicking on the "Buy" button, for the item
     *  items[itemIndex]
     **************************************************************/
    private void buyItem(int itemIndex)
    {
        // if the user has enough coins to buy the item
        if (items[itemIndex].price <= GameStats.loggedInUser.coins)
        {
            // hide the "Buy" button
            buyItemBTN.SetActive(false);

            // update user's coins:
            // local
            GameStats.loggedInUser.coins = GameStats.loggedInUser.coins - items[itemIndex].price;
            // db
            string request = GlobalVariables.serverAddress + "/usersCollection/coins?username=" + 
                             GameStats.loggedInUser.username + "&amount=-" + items[itemIndex].price.ToString();
            StartCoroutine(Requests.PutRequest(request, ""));  
            // UI
            coinsAmount.text = GameStats.loggedInUser.coins.ToString();

            // add item to user's inventory:
            // local
            GameStats.loggedInUser.inventory.Add(items[itemIndex]._id);
            // db
            request = GlobalVariables.serverAddress + "/usersCollection/inventory?username=" + 
                      GameStats.loggedInUser.username + "&itemId=" + items[itemIndex]._id;
            StartCoroutine(Requests.PutRequest(request, ""));
        }
        else
        {
            // The user doesn't have enough coins to buy the item
            buyErrorMsg.SetActive(true);
        }
    }


    /*************************************************************
     *  shopGetRequest sends request to the server, to get all 
     *  shop items, and populates the shop if it succeeds. 
     **************************************************************/
    private IEnumerator shopGetRequest()
    {
        // Create a new UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Get(GlobalVariables.serverAddress + "/clothesCollection");

        // Send the request and wait for a response
        yield return request.SendWebRequest();

        // Check for errors
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Request error: " + request.error);
        }
        else
        {
            // Get the response text
            string responseText = request.downloadHandler.text;
            if (Requests.IsSuccess(request))
            {
                this.items = JsonConvert.DeserializeObject<Item[]>(responseText);
                PopulateShop();
            }
        }
        request.downloadHandler.Dispose();
        request.Dispose();
    }
}

