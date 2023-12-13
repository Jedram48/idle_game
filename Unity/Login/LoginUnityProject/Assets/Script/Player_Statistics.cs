using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using UnityEngine;

public class Player_Statistics : MonoBehaviour {
    [SerializeField] private float animationSpeed = 1f;
    [SerializeField] private float damage = 1;
    [SerializeField] private float clickDamage = 5;
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
        clickDamage += 2;
        PlayerData data = localDatabase.LoadData();
        data.clickDamage = clickDamage;
        localDatabase.SaveData(data);
    }

    public void addDamage()
    {
        damage += 1;
        PlayerData data = localDatabase.LoadData();
        data.damage = damage;
        localDatabase.SaveData(data);
    }

    public void addSpeed()
    {
        animationSpeed += (float).1;
        PlayerData data = localDatabase.LoadData();
        data.animationSpeed = animationSpeed;
        localDatabase.SaveData(data);
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
