using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class Player : Unit
{
    public Transform[] shootPos;
    public Boundary boundary;
    private Rigidbody2D rigid;
    public int hpOrigin;
    private ItemType shootType;
    private float itemTimer;
    private float itemTimerOrigin;
    private bool isInvincible;
    private float invincibleTimer;
    private float invincibleTimerOrigin;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        hpOrigin = hp;
        shootType = 0;
        fireDelay = 0;
        attackAngle = 0;
        itemTimer = 0;
        itemTimerOrigin = 20;
        isInvincible = false;
        invincibleTimer = 0;
        invincibleTimerOrigin = 1;
	}
	
	void Update ()
    {
        AttackUpdate();

        SlowUpdate();
        
        InvincibleUpdate();

        ItemUpdate();

        //cheat
        if(Input.GetKeyDown(KeyCode.F1))
        {
            isInvincible = true;
            invincibleTimerOrigin = 99999;
        }
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed;

        rigid.position = new Vector2(Mathf.Clamp(rigid.position.x, boundary.xMin, boundary.xMax),
                                     Mathf.Clamp(rigid.position.y, boundary.yMin, boundary.yMax));
    }

    private void SlowUpdate()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameDirector.Instance.BecameSlow();
        }
        else if (Input.GetKey(KeyCode.X))
        {
            GameDirector.Instance.OnSlow();
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            GameDirector.Instance.OutSlow();
        }
    }

    private void AttackUpdate()
    {
        fireDelay += Time.deltaTime;

        if (Input.GetKey(KeyCode.Z) && fireDelay > fireDelayOrigin)
        {
            SoundManager.Instance.PlayShoot();
            if(shootType == 0)
            {
                for (int i = 0; i < 1; i++)
                {
                    spawnBullet(BulletType.Player, attackAngle, 10, 6, shootPos[i]);
                }
            }
            else if(shootType == ItemType.Triple)
            {
                for(int i = 0; i < 3;i++)
                {
                    spawnBullet(BulletType.Player, attackAngle, 10, 6, shootPos[i]);
                }
            }
            fireDelay = 0;
        }
    }

    private void InvincibleUpdate()
    {
        if (!isInvincible) return;

        invincibleTimer += Time.deltaTime;

        if (invincibleTimer > invincibleTimerOrigin)
        {
            animator.SetInteger("PlayerState", 0);
            isInvincible = false;
            invincibleTimer = 0;
        }
    }

    private void ItemUpdate()
    {
        if(shootType != 0)
        {
            itemTimer += Time.deltaTime;

            if(itemTimer > itemTimerOrigin)
            {
                shootType = 0;
                itemTimer = 0;
            }
        }
    }

    public override void DecreaseHp(int damage)
    {
        if (isInvincible) return;

        hp -= damage;

        if(hp <= 0)
        {
            GameDirector.Instance.gameOver.SetActive(true);
            gameObject.SetActive(false);
            Time.timeScale = 0.1f;
            GameDirector.Instance.filter.color = new Color(1, 1, 1, 0);
        }

        isInvincible = true;
        animator.SetInteger("PlayerState", 1);
        GameDirector.Instance.HpUpdate();
        CameraShake.Instance.Shake(0.001f, 10, 0.1f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Item")) return;

        ItemType itemType = col.gameObject.GetComponent<Item>().type;

        if (itemType == ItemType.HP)
        {
            hp = (hp + 30 < hpOrigin) ? hp + 30 : hpOrigin;
            GameDirector.Instance.HpUpdate();
        }
        else
        {
            shootType = itemType;
            itemTimer = 0;
        }


        Destroy(col.gameObject);
    }
}
