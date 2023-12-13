using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadFromDatabase : MonoBehaviour
{
    [SerializeField] SpriteRenderer helmet;
    [SerializeField] List<Sprite> helmetList;
    // [SerializeField] SpriteRenderer boots;
    // [SerializeField] List<Sprite> bootsList;
    [SerializeField] SpriteRenderer shield;
    [SerializeField] List<Sprite> shieldList;
    [SerializeField] SpriteRenderer armor;
    [SerializeField] List<Sprite> armorList;
    [SerializeField] SpriteRenderer weapon;
    [SerializeField] List<Sprite> weaponList;
    LocalDatabase localDatabase;
    
    // Start is called before the first frame update
    void Start()
    {
        localDatabase = gameObject.AddComponent<LocalDatabase>();
        PlayerData data = localDatabase.LoadData();
        helmet.sprite = helmetList[data.helmet];
        // boots.sprite = bootsList[data.boots];
        shield.sprite = shieldList[data.shield];
        armor.sprite = armorList[data.armor];
        weapon.sprite = weaponList[data.weapon];
    }
}
