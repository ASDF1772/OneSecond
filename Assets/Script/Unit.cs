using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected Animator animator;
    public string bulletName;
    public int hp;
    public float speed;
    protected int attackAngle;
    protected float fireDelay;
    public float fireDelayOrigin;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void DecreaseHp(int damage) { }


    public void spawnBullet(BulletType type, int attackAngle, float bulletSpeed, int damage, Transform parent = null)
    {
        GameObject bullet = ObjectPool.Instance.PopFromPool(bulletName);
        bullet.transform.position = ((parent == null)? gameObject.transform : parent).position;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, attackAngle));
        bullet.GetComponent<Bullet>().SetBullet(type, bulletSpeed, damage);
        bullet.tag = gameObject.tag;
    }
}
