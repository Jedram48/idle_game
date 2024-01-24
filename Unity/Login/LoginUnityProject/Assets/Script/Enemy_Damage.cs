using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject waypoint;
    [SerializeField] private GameObject player;
    public float Hp;
    public float maxHp;
    [SerializeField] private int money;
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI moneyText;
    private Animator anim, anim_player;
    [SerializeField] public Player_Statistics statistics;
    LocalDatabase localDatabase;

    private void Start()
    {
        maxHp = Hp;
        anim = GetComponent<Animator>();
        anim_player = player.GetComponent<Animator>();
        localDatabase = gameObject.AddComponent<LocalDatabase>();
    }

    public void Damage()
    {
        Hp -= statistics.clickDamages();
    }

    public void OnDestroy()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        // Hp -= Time.deltaTime * statistics.attackDamage();
        // healthBar.fillAmount = (float)Hp / (float)maxHp;

        if (Hp <= 0)
        {
            anim.SetInteger("state", 4); // Change State to Death
            anim_player.SetInteger("state", 0); // Change Player State to Idle
            PlayerData data = localDatabase.LoadData();
            int tmp = data.score;
            tmp += money;
            data.score = tmp;
            moneyText.text = tmp.ToString();
            localDatabase.SaveData(data);
            enabled = false;
        }
    }
}