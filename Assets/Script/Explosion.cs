using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    SpriteRenderer sprite;

	void Start () {
	}
	
	void Update () {
		
	}

    void Destroy()
    {
        Destroy(gameObject);
    }
}
