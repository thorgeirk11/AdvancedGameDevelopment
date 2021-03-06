﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem Instance;
    ScoreSystem() { }

    void Awake()
    {
        if (Instance != null)
        {
            print("Two Soundmanagers! not good!");
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public GameManager gameManager;
    public LifeText LifeCounter;
    public Text ScoreText;

    private float _score;
    public float Score
    {
        get { return _score; }
        set
        {
            _score = value;
            ScoreText.text = Mathf.FloorToInt(_score).ToString();
        }
    }
    public float EnemyHitScore = 20;
    public float EnemyReachPenalty = 1f;
    public float MultyKillFactor = 1.5f;

    private float topX;

    public Text ScoreSlashPrefab;

    void Start()
    {
        var spawnPosition = new Vector3(0, Screen.height);
        var worldPoint = Camera.main.ScreenToWorldPoint(spawnPosition);
        topX = worldPoint.x;
    }

    public void WeaponHitEnemy(Enemy enemy, Weapon weapon, Collision2D hit)
    {
        weapon.hitCount++;
        var hits = weapon.hitCount;
        var velocity = hit.relativeVelocity.magnitude;

        var pointsForHit = weapon.hitCount * velocity;

        Score += pointsForHit;

        var scoreSplash = SpawnScoreSplash(enemy);
        scoreSplash.text = (hits > 1 ? hits + "x" : "") + velocity.ToString("##");
    }

    private Text SpawnScoreSplash(Enemy enemy)
    {
        var toScreenPosition = Camera.main.WorldToScreenPoint(enemy.transform.position);
        var scoreSplash = Instantiate(ScoreSlashPrefab, toScreenPosition, Quaternion.identity);
        scoreSplash.rectTransform.SetParent(transform);
        return scoreSplash;
    }

    internal void EnemyHitDefence(Enemy enemy)
    {
        LifeCounter.Lifes--;
        if (LifeCounter.Lifes == 0)
        {
            gameManager.LostGame();
        }
    }
}
