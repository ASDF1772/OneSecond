using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject player;
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    private float spawnTimer;
    private int spawnPhase;
    private bool onCoroutine;
    
    void Start () {
        onCoroutine = false;
        spawnPhase = 1;
	}
	
	void Update ()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > 8 && !onCoroutine)
        {
            if(spawnPhase == 1)
            {
                StartCoroutine("PhaseOne");
            }
            else if(spawnPhase == 2)
            {
                StartCoroutine("PhaseTwo");
            }
            else if(spawnPhase == 3)
            {
                StartCoroutine("PhaseThree");
            }
            else if(spawnPhase == 4)
            {
                GameObject boss = Instantiate(bossPrefab, new Vector2(0, 6), Quaternion.identity);
                boss.GetComponent<Boss>().player = player;
            }
            onCoroutine = true;
            spawnPhase++;
            spawnTimer = 0;
        }
    }

    IEnumerator PhaseOne()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = -1; j < 2; j+=2)
            {
                GameObject e = Instantiate(enemyPrefab, new Vector2(j * (1 + i), 6), Quaternion.identity);
                e.GetComponent<Enemy>().SetEnemy(player, 1, 270);
            }
            yield return new WaitForSeconds(2f);
        }
        onCoroutine = false;
    }

    IEnumerator PhaseTwo()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = -1; j < 2; j += 2)
            {
                GameObject e = Instantiate(enemyPrefab, new Vector2(3 * j, 6), Quaternion.identity);
                e.GetComponent<Enemy>().SetEnemy(player, 2, 270 - j * 30);
            }
            yield return new WaitForSeconds(2f);
        }
        onCoroutine = false;
    }

    IEnumerator PhaseThree()
    {
        for(int i = 0; i < 20; i++)
        {
            GameObject e = Instantiate(enemyPrefab, new Vector2(Random.Range(-2f, 2f), 6), Quaternion.identity);
            e.GetComponent<Enemy>().SetEnemy(player, Random.Range(1, 3), 270 + Random.Range(-30, 30));

            yield return new WaitForSeconds(1.5f);
        }
        onCoroutine = false;
    }
}
