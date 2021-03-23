using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PooledObject
{
    [SerializeField]
    public string itemName;
    public GameObject itemPrefab;
    public int poolCount;

    [SerializeField]
    private List<GameObject> poolList = new List<GameObject>();

    public void Initialize(Transform parent = null)
    {
        for(int i = 0; i<poolCount;i++)
        {
            poolList.Add(CreateItem(parent));
        }
    }

    public void PushToPool(GameObject item, Transform parent = null)
    {
        item.transform.SetParent(parent);
        item.SetActive(false);
        poolList.Add(item);
    }

    public GameObject PopFromPool(Transform parent = null)
    {
        if(poolList.Count == 0)
        {
            poolList.Add(CreateItem(parent));
        }
        GameObject item = poolList[0];
        item.SetActive(true);
        poolList.RemoveAt(0);

        return item;
    }

    public GameObject CreateItem(Transform parent = null)
    {
        GameObject item = Object.Instantiate(itemPrefab) as GameObject;
        item.name = itemName;
        item.transform.SetParent(parent);
        item.SetActive(false);

        return item;
    }
}
