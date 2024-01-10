using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using UnityEngine;

public class Player_Statistics : MonoBehaviour {
    [SerializeField] private float animationSpeed = 1f;
    [SerializeField] private float damage = 1;
    [SerializeField] private float clickDamage = 5;
    [SerializeField] ShopPrices shopPrices;
    private Animator anim;
    LocalDatabase localDatabase;

    // Start is called before the first frame update
    void Start()
    {
        localDatabase = gameObject.AddComponent<LocalDatabase>();
        PlayerData data = localDatabase.LoadData();
        animationSpeed = data.animationSpeed;
        damage = data.damage;
        clickDamage = data.clickDamage;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.speed = animationSpeed;
    }
    public void addClickDamage()
    {
        PlayerData data = localDatabase.LoadData();
        
        if (shopPrices.getPrice(1) <= data.score)
        {
            clickDamage += 2;
            data.score -= shopPrices.getPrice(1);
            data.clickDamage = clickDamage;
            localDatabase.SaveData(data);
            shopPrices.updatePrice();
        }
    }

    public void addDamage()
    {
        PlayerData data = localDatabase.LoadData();
        if (shopPrices.getPrice(0) <= data.score)
        {
            damage += 1;
            data.score -= shopPrices.getPrice(0);
            data.damage = damage;
            localDatabase.SaveData(data);
            shopPrices.updatePrice();
        }
    }

    public void addSpeed()
    {
        PlayerData data = localDatabase.LoadData();
        if (shopPrices.getPrice(2) <= data.score)
        {
            animationSpeed += (float).05;
            data.score -= shopPrices.getPrice(2);
            data.animationSpeed = animationSpeed;
            localDatabase.SaveData(data);
            shopPrices.updatePrice();
        }
        
    }
    public float clickDamages()
    {
        return clickDamage;
    }

    public float attackDamage()
    {
        return animationSpeed * damage;
    }
}
