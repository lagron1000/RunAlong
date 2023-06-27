using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// mutual functionalities of the shop and the inventory screens

public class ShopAndInventory : MonoBehaviour
{
    /*************************************************************
     *  Dress the charater in its current outfit
     **************************************************************/
    public static void wearCurrOutfit(GameObject character)
    {
        DressUp("Top", GameStats.loggedInUser.top, character);
        DressUp("Bottom", GameStats.loggedInUser.bottom, character);
        DressUp("Shoes", GameStats.loggedInUser.shoes, character);
    }


    /*************************************************************
     *  Dress the charater in the given item
     *  id - the id of the item
     *  type - the type of the item (top / bottom / shoes)
     **************************************************************/
    public static void DressUp(string type, string id, GameObject character)
    {
        Transform clothesParentContainer = character.transform.Find(type);
        Transform oldClothes = clothesParentContainer.GetChild(0);

        GameObject newClothes = Instantiate(ShopItemsCollection.GetModel(id), 
                                            oldClothes.transform.position, 
                                            oldClothes.transform.rotation, 
                                            clothesParentContainer);

        newClothes.transform.localScale = oldClothes.localScale;

        if (clothesParentContainer.childCount > 0)
        {
            Destroy(clothesParentContainer.GetChild(0).gameObject);
        }

        newClothes.transform.SetParent(clothesParentContainer);
    }
}

