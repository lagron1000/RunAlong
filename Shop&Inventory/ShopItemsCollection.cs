using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemsCollection
{
    // IDS
    const string MINT_SHIRT_ID = "63ff6c98add07a32333307bb";
    const string BLUE_PANTS_ID = "642c52695f25c89505b00f7c";
    const string DARK_GREEN_SHIRT_ID = "6454d71001ba82fa1931ea4f";
    const string DESERT_SHIRT_ID = "6454d78e01ba82fa1931ea52";
    const string WHITE_SHIRT_ID = "6454d76001ba82fa1931ea50";
    const string SANDALS_ID = "6454d82501ba82fa1931ea56";
    const string GREY_PANTS_ID = "6454d79c01ba82fa1931ea53";
    const string BLACK_LEATHER_PANTS_ID = "6454d7f201ba82fa1931ea55";
    const string WOODY_SHIRT_ID = "6454d7b101ba82fa1931ea54";
    const string ORANGE_PANTS_ID = "6454d77301ba82fa1931ea51";


    public class ShopItemObj
    {
        public GameObject Model { get; set; }
        public Texture2D Image { get; set; }
        public Material Material { get; set; }
    }

    private static Dictionary<string, ShopItemObj> shopItemsDictionary = new Dictionary<string, ShopItemObj>()
    {
        { MINT_SHIRT_ID, new ShopItemObj
            {
                Model = Resources.Load<GameObject>("Models/Clothes/GreenShortShirt"),
                Image = Resources.Load<Texture2D>("clothes_images/GreenShirtImg"),
                Material = Resources.Load<Material>("Models/Clothes/GreenFabric")
            }
        },
        { BLUE_PANTS_ID, new ShopItemObj
            {
                Model = Resources.Load<GameObject>("Models/Clothes/BlueShortPants"),
                Image = Resources.Load<Texture2D>("clothes_images/ShortBluePantsImg"),
                Material = Resources.Load<Material>("Models/Clothes/BlueFabric")
            }
        },
        { DARK_GREEN_SHIRT_ID, new ShopItemObj
            {
                Model = Resources.Load<GameObject>("Models/Clothes/DarkGreenShortShirt"),
                Image = Resources.Load<Texture2D>("clothes_images/DarkGreenShirtImg"),
                Material = Resources.Load<Material>("Models/Clothes/DarkGreenFabric")
            }
        },
        { DESERT_SHIRT_ID, new ShopItemObj
            {
                Model = Resources.Load<GameObject>("Models/Clothes/DesertShirt"),
                Image = Resources.Load<Texture2D>("clothes_images/desertShirtImg"),
                Material = Resources.Load<Material>("Models/Clothes/Ground - Surface Shader Scene")
            }
        },
        { WHITE_SHIRT_ID, new ShopItemObj
            {
                Model = Resources.Load<GameObject>("Models/Clothes/WhiteShortShirt"),
                Image = Resources.Load<Texture2D>("clothes_images/whiteShirtImg"),
                Material = Resources.Load<Material>("Models/Clothes/WhiteFabric")
            }
        },
        { SANDALS_ID, new ShopItemObj
            {
                Model = Resources.Load<GameObject>("Models/Clothes/LeatherSandals"),
                Image = Resources.Load<Texture2D>("clothes_images/leatherSandalsImg"),
                Material = Resources.Load<Material>("Models/Clothes/BrownFabric")
            }
        },
        { GREY_PANTS_ID, new ShopItemObj
            {
                Model = Resources.Load<GameObject>("Models/Clothes/GreyShortPants"),
                Image = Resources.Load<Texture2D>("clothes_images/ShortGreyPantsImg"),
                Material = Resources.Load<Material>("Models/Clothes/GreyFabric")
            }
        },
        { BLACK_LEATHER_PANTS_ID, new ShopItemObj
            {
                Model = Resources.Load<GameObject>("Models/Clothes/LeatherShortPants"),
                Image = Resources.Load<Texture2D>("clothes_images/ShortBlackPantsImg"),
                Material = Resources.Load<Material>("Models/Clothes/eyes.012")
            }
        },
        { WOODY_SHIRT_ID, new ShopItemObj
            {
                Model = Resources.Load<GameObject>("Models/Clothes/WoodyShortShirt"),
                Image = Resources.Load<Texture2D>("clothes_images/ShortWoodyShirtImg"),
                Material = Resources.Load<Material>("Models/Clothes/WoodyFabric")
            }
        },
        { ORANGE_PANTS_ID, new ShopItemObj
            {
                Model = Resources.Load<GameObject>("Models/Clothes/OrangeShortPants"),
                Image = Resources.Load<Texture2D>("clothes_images/ShortOrangePantsImg"),
                Material = Resources.Load<Material>("Models/Clothes/OrangeFabric")
            }
        },
    };


    /*************************************************************
    *  Get model given shop item id
    **************************************************************/
    public static GameObject GetModel(string id)
    {
        return shopItemsDictionary[id].Model;
    }


    /*************************************************************
    *  Get image given shop item id
    **************************************************************/
    public static Sprite GetImage(string id)
    {
        Texture2D texture = shopItemsDictionary[id].Image;
        Sprite image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        return image;
    }


    /*************************************************************
    *  Get material given shop item id
    **************************************************************/
    public static Material GetMaterial(string id)
    {
        return shopItemsDictionary[id].Material;
    }


    /*************************************************************
    *  Get random material
    **************************************************************/
    static string[] ids = { MINT_SHIRT_ID, BLUE_PANTS_ID, DARK_GREEN_SHIRT_ID, DESERT_SHIRT_ID, WHITE_SHIRT_ID,
                            SANDALS_ID, GREY_PANTS_ID, BLACK_LEATHER_PANTS_ID, WOODY_SHIRT_ID, ORANGE_PANTS_ID };

    public static Material getRandMaterial()
    {
        return shopItemsDictionary[ids[Random.Range(0, ids.Length)]].Material;
    }
}
