using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")] 
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 20f;

    [Header("# Player Info")] 
    public int health;
    public int maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp;
    
    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;

    void Awake()
    {
        instance = this;
        // if 
    }

    private void Start()
    {
        health = maxHealth;
        
        // 임시용 코드
        uiLevelUp.Select(0);
    }

    private void Update()
    {
        if (!isLive)
            return;
        
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

    public void GetExp()
    {
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
