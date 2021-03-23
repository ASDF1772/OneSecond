using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDirector : Singleton<GameDirector>
{
    private int score;
    public Text scoreText;
    public GameObject info;
    public GameObject gameOver;
    public GameObject clear;
    public Image filter;
    public Image abilityBar;
    public Image hpBar;
    public GameObject player;
    private float slowTimer;
    private float slowTimerMax;
    private bool canUseSlow;
    private bool isOutSlow;
    private bool isFadeIn;
    
    void Start() {
        score = 0;
        slowTimer = 1000f;
        slowTimerMax = 1000f;
        canUseSlow = true;
        isFadeIn = false;
        isOutSlow = false;
        StartCoroutine("ShowInfo");
	}
	
	void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("GameScene");
        }

        if(isOutSlow)
        {
            float alphaValue = filter.color.a;
            if (alphaValue < 0.08f)
            {
                alphaValue += Time.deltaTime;

                if (alphaValue > 0.08f)
                {
                    alphaValue = 0.08f;
                }
                filter.color = new Color(1, 1, 1, alphaValue);
            }

            slowTimer += Time.deltaTime / 4;
            abilityBar.fillAmount = slowTimer / slowTimerMax;

            if(slowTimer > slowTimerMax)
            {
                abilityBar.fillAmount = 1;
                slowTimer = slowTimerMax;
                isOutSlow = false;
                canUseSlow = true;

                if(Input.GetKey(KeyCode.X))
                {
                    BecameSlow();
                }
            }
        }
	}

    public void BecameSlow()
    {
        if (!canUseSlow) return;

        isOutSlow = false;
        Time.timeScale = 0.5f;
        player.GetComponent<Player>().speed = 6;
    }

    public void OnSlow()
    {
        if (!canUseSlow) return;

        slowTimer -= Time.deltaTime / Time.timeScale;
        abilityBar.fillAmount = slowTimer / slowTimerMax;

        float alphaValue = filter.color.a;

        if (alphaValue > 0)
        {
            alphaValue -= Time.deltaTime;

            if (alphaValue < 0)
            {
                alphaValue = 0;
            }
            filter.color = new Color(1, 1, 1, alphaValue);
        }

        if (slowTimer < 0)
        {
            isOutSlow = true;
            canUseSlow = false;
            OutSlow();
            StartCoroutine("RunFade");
        }
    }

    public void OutSlow()
    {
        isOutSlow = true;
        Time.timeScale = 1;
        player.GetComponent<Player>().speed = 3;
    }

    IEnumerator RunFade()
    {
        for (int i = 0; i < slowTimerMax / 0.1f - 1; i++)
        {
            abilityBar.CrossFadeAlpha(isFadeIn ? 0.4f : 1f, 0.4f, true);
            isFadeIn = !isFadeIn;
            yield return new WaitForSeconds(0.4f);
        }
        isFadeIn = false;
        //bar.CrossFadeAlpha(1f, 0.1f, true);
    }

    public void HpUpdate()
    {
        hpBar.fillAmount = (float)player.GetComponent<Player>().hp / player.GetComponent<Player>().hpOrigin;
    }

    public void AddScore(int num)
    {
        score += num;
        scoreText.text = "Score\n\n" + score;
    }

    IEnumerator ShowInfo()
    {
        yield return new WaitForSeconds(4);

        info.SetActive(false);
    }

    IEnumerator Clear()
    {
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene("MainScene");
    }
}