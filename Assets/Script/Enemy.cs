using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public GameObject itemPrefab;
    public GameObject player;
    private int type;
    private Vector2 dir;

    void Start ()
    {
        fireDelay = Random.Range(0f, 0.4f);
        attackAngle = 180;
    }

    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);

        if(-4 < transform.position.y && transform.position.y < 5)
        {
            if (-5 < transform.position.x && transform.position.x < 5)
            {
                AttackUpdate();
            }
        }
        else if(-6 > transform.position.y)
        {
            Destroy(gameObject);
        }
    }

    public override void DecreaseHp(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            if (Random.Range(0, 10) == 0)
            {
                GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
                item.GetComponent<Item>().SetItem((ItemType)Random.Range(0, 2));
            }
            GameDirector.Instance.AddScore(100);
            Destroy(gameObject);
        }
    }

    private void AttackUpdate()
    {
        fireDelay += Time.deltaTime;
        

        if (fireDelay > fireDelayOrigin)
        {
            if (type == 1)
            {
                Vector2 v = transform.position - player.transform.position;
                attackAngle = 90 + (int)(Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg);
                spawnBullet(BulletType.Enemy, attackAngle, 5, 10);
            }
            else if(type == 2)
            {
                for (int i = -1; i <= 1; i++)
                {
                    spawnBullet(BulletType.Enemy, attackAngle + i * 30, 5, 10);
                }
            }
            SoundManager.Instance.PlayShoot();
            fireDelay = 0;
        }
    }

    public void SetEnemy(GameObject player, int type, int dir)
    {
        this.dir = new Vector2(Mathf.Cos(Mathf.Deg2Rad * dir), Mathf.Sin(Mathf.Deg2Rad * dir));
        this.player = player;
        this.type = type;
        if(type == 1)
        {
            fireDelayOrigin = 1;
        }
        else if(type == 2)
        {
            fireDelayOrigin = 2;
        }
    }
}
