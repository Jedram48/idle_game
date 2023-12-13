using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string name;
    public int score;
    public float animationSpeed;
    public float damage;
    public float clickDamage;
    public int helmet;
    public int boots;
    public int shield;
    public int armor;
    public int weapon;
}

public class LocalDatabase : MonoBehaviour
{
    private string filePath;

    // Start is called before the first frame update
    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        PlayerData data = LoadData();
        if (data == null)
        {
            SaveData(new PlayerData { name = "Player", score = 0, animationSpeed = 1f, damage = 1f, clickDamage = 5f, helmet = 0, boots = 0, shield = 0, armor = 0, weapon = 0});
        }
        // PlayerData loadedData = LoadData();
        // Debug.Log(loadedData);
    }

    public void SaveData(PlayerData newData)
    {
        string jsonData = JsonUtility.ToJson(newData);
        string filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        File.WriteAllText(filePath, jsonData);
    }

    public PlayerData LoadData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<PlayerData>(jsonData);
        }
        return null;
    }
}
