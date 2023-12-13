using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class LoadScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    LocalDatabase localDatabase;

    // Start is called before the first frame update
    void Start()
    {
        localDatabase = gameObject.AddComponent<LocalDatabase>();
        PlayerData data = localDatabase.LoadData();
        text.text = data.score.ToString();
    }
}
