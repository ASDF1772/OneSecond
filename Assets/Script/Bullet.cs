using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Player,
    Enemy
}

public class Bullet : MonoBehaviour 
{
    private string itemName = "Bullet";
    private GameObject explosionPrefab;
    private SpriteRenderer spriteRenderer;
    private Sprite[] sprite;
    private BulletType type;
    private int damage;
    private float speed;
    
	void Start () 
    {
	}

    void FixedUpdate()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    public void SetBullet(BulletType type, float speed, int damage)
    {
        spriteRenderer.sprite = sprite[(int)type];
        this.type = type;
        this.speed = speed;
        this.damage = damage;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag != "Screen") return;

        
        ObjectPool.Instance.PushToPool(itemName, gameObject);
        //Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (gameObject.CompareTag(col.gameObject.tag)) return;
        
        if(col.gameObject.tag == "Ally" || col.gameObject.tag == "Enemy")
        {
            CameraShake.Instance.Shake(0.001f, 3, 0.12f);
        }
        else
        {
            return;
        }

        SoundManager.Instance.PlayExplosion();
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        col.GetComponent<Unit>().DecreaseHp(damage);
        ObjectPool.Instance.PushToPool(itemName, gameObject);
        return;

        //튕기기
        //float tmp = Mathf.Atan2(col.transform.position.y - transform.position.y, col.transform.position.x - transform.position.x);
        //tmp = Mathf.Rad2Deg * tmp + Random.Range(0f, 2f) * 90;
        //transform.Rotate(new Vector3(0, 0, tmp));
    }
}
