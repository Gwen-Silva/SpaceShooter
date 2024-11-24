using UnityEngine;
using TMPro;

public class ScoreManager : Singleton<ScoreManager>
{
    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;

    [Header("Score Settings")]
    public int score = 0;
    public int pointsPerSecond = 10;
    public int pointsPerKill = 50;
    public int pointsLostOnEscape = 50;

    [Header("Player Reference")]
    public Player player;

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
