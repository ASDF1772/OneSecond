using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
	private Vector3 originalPos;

    private float duration;
    private float durationOrigin;
    private int count;
    private int countOrigin;
	private float range;
    private bool isShake = false;
	
    void Start()
    {
        originalPos = transform.position;
    }
 
	void Update()
	{
        if (isShake)
        {
            duration += Time.deltaTime;

            if (duration >= durationOrigin)
            {
                transform.Translate(Random.insideUnitSphere * range);
                duration = 0;

                if (++count >= countOrigin)
                {
                    transform.position = originalPos;
                    isShake = false;
                }
            }
        }
	}

    public void Shake(float duration, int count, float range)
    {
        this.duration = 0;
        this.durationOrigin = duration;
        this.count = 0;
        this.countOrigin = count;
        this.range = range;
        this.isShake = true;
    }
}
