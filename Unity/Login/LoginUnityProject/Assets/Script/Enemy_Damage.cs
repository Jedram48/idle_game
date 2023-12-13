using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour {
    [SerializeField] private GameObject waypoint;
    [SerializeField] private GameObject player;
    [SerializeField] private float Hp;
    [SerializeField] private int money;
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI moneyText;
    private float maxHp;
    private Animator anim, anim_player;
    [SerializeField] private Player_Statistics statistics;
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
        healthBar.fillAmount = (float)Hp / (float)maxHp;
    }

    private void Update()
    {
        if (Vector2.Distance(waypoint.transform.position, transform.position) < .1f)
        {
            Hp -= Time.deltaTime * statistics.attackDamage();
            healthBar.fillAmount = (float)Hp / (float)maxHp;

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
}