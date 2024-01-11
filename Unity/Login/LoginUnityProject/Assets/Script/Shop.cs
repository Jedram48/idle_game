using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] List<Sprite> list;
    [SerializeField] SpriteRenderer obj;
    [SerializeField] RectTransform iconImage;
    [SerializeField] ObjectType objType;
    [SerializeField] ShopPrices shopPrices;

    enum ObjectType{ helmet, boots, shield, armor, weapon } 
    LocalDatabase localDatabase;
    Image image;
    int i;
    // Start is called before the first frame update
    void Start()
    {
        image = iconImage.GetComponent<Image>();
        localDatabase = gameObject.AddComponent<LocalDatabase>();

        PlayerData data = localDatabase.LoadData();
        switch (objType)
        {
            case ObjectType.helmet:
                i = data.helmet;
                break;
            case ObjectType.boots:
                i = data.boots;
                break;
            case ObjectType.shield:
                i = data.shield;
                break;
            case ObjectType.armor:
                i = data.armor;
                break;
            case ObjectType.weapon:
                i = data.armor;
                break;
        }

        if (i >= list.Count)
        {
            gameObject.SetActive(false);
        }
        else
        {
            image.sprite = list[i];
        }
    }

    public void click()
    {
        if (i < list.Count)
        {
            int price = shopPrices.getPrice((int)objType + 3);
            PlayerData data = localDatabase.LoadData();

            if (price <= data.score)
            {
                obj.sprite = list[i];
                i++;

                data.score -= price;

                switch (objType)
                {
                    case ObjectType.helmet:
                        data.helmet = i;
                        break;
                    case ObjectType.boots:
                        data.boots = i;
                        break;
                    case ObjectType.shield:
                        data.shield = i;
                        break;
                    case ObjectType.armor:
                        data.armor = i;
                        break;
                    case ObjectType.weapon:
                        data.weapon = i;
                        break;
                }
                localDatabase.SaveData(data);

                if (i == list.Count)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    image.sprite = list[i];
                }
                shopPrices.updatePrice(); 
            }
        }
    }
}
