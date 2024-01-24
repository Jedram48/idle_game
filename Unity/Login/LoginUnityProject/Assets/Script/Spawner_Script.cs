using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Enemy entryEnemy;
    public Enemy enemy;
    public Enemy Boss;
    private int win_count = 0;

    // Start is called before the first frame update
    void Start()
    {
        enemy = Instantiate(entryEnemy, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.Hp <= 0)
        {
            Destroy(enemy);
            if (win_count == 5)
            {
                enemy = Instantiate(Boss, transform.position, Quaternion.identity);
            }
            else
            {
                enemy = Instantiate(entryEnemy, transform.position, Quaternion.identity);
            }
            win_count++;
        }
    }

    public void Damage()
    {
        enemy.Damage();
    }
}
