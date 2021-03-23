using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Unit
{
    public GameObject player;
    public Transform[] shootPos;
    int state;
    int phase;
    int shootPosIndex;
    
	void Start () {
        fireDelay = 0;
        attackAngle = 0;
        shootPosIndex = 0;
        state = 0;
        phase = 3;
    }
	
	void Update ()
    {
        if(transform.position.y > 1)
        {
            transform.Translate(Vector2.down * Time.deltaTime);
        }
        else
        {
            AttackUpdate();
        }
    }

    public override void DecreaseHp(int damage)
    {
        hp -= damage;

        if(hp <= 0)
        {
            Destroy(gameObject);
            GameDirector.Instance.AddScore(1000);
            GameDirector.Instance.clear.SetActive(true);
            GameDirector.Instance.StartCoroutine("Clear");
        }
    }

    private void AttackUpdate()
    {
        if (state != 0) return;

        fireDelay += Time.deltaTime;

        if (fireDelay > fireDelayOrigin)
        {
            if(phase == 1)
            {
                state = 1;
                StartCoroutine("PhaseOne");
            }
            else if(phase == 2)
            {
                state = 2;
                StartCoroutine("PhaseTwo");
            }
            else if(phase == 3)
            {
                state = 1;
                StartCoroutine("PhaseThree");
            }
            fireDelay = 0;
            animator.SetInteger("BossState", state);
        }
    }

    IEnumerator PhaseOne()
    {
        while (attackAngle < 180)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    spawnBullet(BulletType.Enemy, i * 120 + j * 10 + attackAngle, 8 - j, 15, shootPos[0]);
                }
            }
            attackAngle += 7;
            yield return new WaitForSeconds(0.2f);
        }
        while (attackAngle > 0)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    spawnBullet(BulletType.Enemy, i * 120 + j * 10 + attackAngle, 6 + j, 15, shootPos[0]);
                }
            }
            attackAngle -= 7;
            yield return new WaitForSeconds(0.2f);
        }

        AttackEnd();
    }

    IEnumerator PhaseTwo()
    {
        yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 36; j++)
            {
                spawnBullet(BulletType.Enemy, j * 10 + i * 3, 4, 15, shootPos[shootPosIndex]);
            }
            shootPosIndex = (++shootPosIndex > 1) ? 0 : shootPosIndex;

            if (i == 5)
            {
                AttackEnd();
            }
            yield return new WaitForSeconds(0.4f);
        }
    }

    IEnumerator PhaseThree()
    {
        yield return new WaitForSeconds(0.4f);
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 v = shootPos[0].position - player.transform.position;
                attackAngle = Random.Range(-20, 20) + 90 + (int)(Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg);
                spawnBullet(BulletType.Enemy, attackAngle, 10 + Random.Range(-4, 4), 15, shootPos[0]);
            }
            yield return new WaitForSeconds(1f);
        }
        AttackEnd();
    }

    private void AttackEnd()
    {
        attackAngle = 0;
        fireDelay = 0;
        phase = (++phase > 3)? 1 : phase;
        state = 0;
        animator.SetInteger("BossState", state);
    }
}
