using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager> {

    public AudioClip explosionSound;
    public AudioClip shootSound;
    public AudioSource explosionSource;
    public AudioSource shootSource;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayExplosion()
    {
        explosionSource.PlayOneShot(explosionSound);
    }

    public void PlayShoot()
    {
        shootSource.PlayOneShot(shootSound);
    }
}
