using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    HP,
    Triple
}

public class Item : MonoBehaviour
{
    public ItemType type;
    public Sprite[] sprite;

	void Start () {
        SetItem(type);
	}
	
	void Update () {
		transform.Translate(Vector2.down * 2 * Time.deltaTime);
	}

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void SetItem(ItemType type)
    {
        this.type = type;
        GetComponent<SpriteRenderer>().sprite = sprite[(int)type];
    }
}
