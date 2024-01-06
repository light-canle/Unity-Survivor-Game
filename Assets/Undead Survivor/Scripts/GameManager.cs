using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")] 
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 60f;

    [Header("# Player Info")] 
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp;
    
    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject enemyCleaner;

    void Awake()
    {
        instance = this;
        // if 
    }

    public void GameStart()
    {
        health = maxHealth;
        
        uiLevelUp.Select(0); // 임시용 코드
        Resume();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        
        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);
        
        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();
    }
    
    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (!isLive)
            return;
        
        gameTime += Time.deltaTime;
        
        // 최대 시간까지 버티면 이김
        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }
    }

    public void GetExp()
    {
        // enemyCleaner에 의해 적이 죽은 경우에는 경험치를 얻지 않음
        if (!isLive)
            return;
        exp++;

        if (exp >= nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            level++;
            exp = 0;
            // 레벨 업 UI 표시
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        // 시간 속도 조절
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        // 시간 속도 조절
        Time.timeScale = 1f;
    }
}
