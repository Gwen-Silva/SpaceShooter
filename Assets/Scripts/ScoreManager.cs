﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import necessário para UI

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI Elements")]
    public Text scoreText;

    [Header("Score Settings")]
    public int score = 0;
    public int pointsPerSecond = 10;
    public int pointsPerKill = 50;
    public int pointsLostOnEscape = 50;

    private void Awake()
    {
        // Singleton pattern to ensure uma única instância
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Incrementar pontos ao longo do tempo
        InvokeRepeating(nameof(AddSurvivalPoints), 1f, 1f);
        UpdateScoreUI();
    }

    private void AddSurvivalPoints()
    {
        score += pointsPerSecond;
        UpdateScoreUI();
    }

    public void AddKillPoints()
    {
        score += pointsPerKill;
        UpdateScoreUI();
    }

    public void DeductEscapePoints()
    {
        score -= pointsLostOnEscape;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }
}
