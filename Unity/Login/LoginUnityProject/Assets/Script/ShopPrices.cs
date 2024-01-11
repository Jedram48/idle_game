using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ShopPrices : MonoBehaviour
{   
    [SerializeField] List<TextMeshProUGUI> list;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] ObjectType objType;

    private enum ObjectType{ damage, click, speed, helmet, boots, shield, armor, weapon }
    LocalDatabase localDatabase;
    private PlayerData data;

    // Start is called before the first frame update
    void Start()
    {
        localDatabase = gameObject.AddComponent<LocalDatabase>();
        
        data = localDatabase.LoadData();
        foreach (ObjectType type in Enum.GetValues(typeof(ObjectType)))
        {
            list[(int)type].text = price(type);
        }
    }

    private string price(ObjectType type)
    {
        int p = 0;
        float t;

        switch (type)
        {
            case ObjectType.damage:
                t = data.damage;
                p = (int)(t-1);
                break;
            case ObjectType.click:
                t = data.clickDamage;
                p = (int)((t-5)/2);
                break;
            case ObjectType.speed:
                t = data.animationSpeed;
                p = (int)((t-1)*20);
                break;
            case ObjectType.helmet:
                t = data.helmet;
                p = (int)t;
                break;
            case ObjectType.boots:
                t = data.boots;
                p = (int)t;
                break;
            case ObjectType.shield:
                t = data.shield;
                p = (int)t;
                break;
            case ObjectType.armor:
                t = data.armor;
                p = (int)t;
                break;
            case ObjectType.weapon:
                t = data.weapon;
                p = (int)t;
                break;
        }

        return (100*Math.Pow(2, p)).ToString();
    }

    public void updatePrice()
    {
        data = localDatabase.LoadData();
        score.text = data.score.ToString();
        foreach (ObjectType type in Enum.GetValues(typeof(ObjectType)))
        {
            list[(int)type].text = price(type);
        }
    }

    public int getPrice(int type)
    {
        data = localDatabase.LoadData();
        return int.Parse(price((ObjectType)type));
    }
}
