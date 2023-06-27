using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Networking;
using Newtonsoft.Json;


public class InventoryScript : MonoBehaviour
{
    public GameObject shopItemPrefab;
    public Transform itemsContainer;
    private Item[] items;
    private string currTopId;
    private string currBottomId;
    private string currShoesId;
    public GameObject character;
    public GameObject backBTN;


    /**************************************************************
     * Start is called before the first frame update
     **************************************************************/
    void Start()
    { 
        Button btn = backBTN.GetComponent<Button>();
        btn.onClick.AddListener(BackToMenu);
    }


    /*************************************************************
     *  OnEnable function is called when the GameObject or 
     *  Component becomes active or enabled
     **************************************************************/
    void OnEnable()
    {
        StartCoroutine(inventoryGetRequest());
        currTopId = GameStats.loggedInUser.top;
        currBottomId = GameStats.loggedInUser.bottom;
        currShoesId = GameStats.loggedInUser.shoes;
        ShopAndInventory.wearCurrOutfit(character);
    }


    /*************************************************************
     *  Action of clicking on the "Back" button
     **************************************************************/
    private void BackToMenu()
    {
        // set the current clothes as the new outfit:
        // local
        GameStats.loggedInUser.top = currTopId;
        GameStats.loggedInUser.bottom = currBottomId;
        GameStats.loggedInUser.shoes = currShoesId;
        // DB
        string jsonData = "{\"top\":\"" + currTopId + 
                          "\",\"bottom\":\"" + currBottomId + 
                          "\",\"shoes\":\"" + currShoesId + "\"}";

        string request = GlobalVariables.serverAddress + 
                         "/usersCollection/outfit?username=" + 
                         GameStats.loggedInUser.username;

        StartCoroutine(Requests.PutRequest(request, jsonData));

        // back to main menu
        GameObject gameMainMenu = GameObject.Find("GameMainMenu");
        if (gameMainMenu != null)
        {
            gameMainMenu.transform.GetChild(1).gameObject.SetActive(true);
            GameObject.Find("InventoryScreen").SetActive(false);
        }
        else
        {
            Debug.Log("gameMainMenu is null");
        }
    }


    /*************************************************************
     *  View the inventory of the loggeed-in user
     **************************************************************/
    private void PopulateInventory()
    {
        // Clear existing UI objects in itemsContainer
        foreach (Transform child in itemsContainer)
        {
            Destroy(child.gameObject);
        }

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
            obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
        }
    }


    /*************************************************************
     *  Action of clicking on an item from the inventory
     **************************************************************/
    private void OnItemClick(int itemIndex)
    {
        Item item = items[itemIndex];
        ShopAndInventory.DressUp(item.type, item._id, character);

        switch (item.type)
        {
            case "Top":
                currTopId = items[itemIndex]._id;
                break;
            case "Bottom":
                currBottomId = items[itemIndex]._id;
                break;
            case "Shoes":
                currShoesId = items[itemIndex]._id;
                break;
            default:
                Debug.LogError("Invalid category index!");
                return;
        }
    }


    /*************************************************************
     *  inventoryGetRequest sends request to the server, to get
     *  the inventory of the logged-in user, and populates the 
     *  inventory if it succeeds. 
     **************************************************************/
    private IEnumerator inventoryGetRequest()
    {
        // Create a new UnityWebRequest object
        UnityWebRequest request = UnityWebRequest.Get(GlobalVariables.serverAddress + 
                                  "/clothesCollection/inventory/" + 
                                  GameStats.loggedInUser.username);

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
                PopulateInventory();
            }

        }
        request.downloadHandler.Dispose();
        request.Dispose();
    }
}
