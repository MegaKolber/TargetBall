using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    public TextMeshPro timeText;
    public TextMeshPro scoreText;
    public TextMeshPro bestText;

    float timeRemaining = 60f;
    bool timerRunning = false;

    int score = 0;
    int best = 0;

    void Start()
    {
        UpdateTimeText();
        UpdateScoreText();
        UpdateBestText();
    }

    void Update()
    {
        if (!timerRunning) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            timerRunning = false;
            // end round here if you want
        }


        UpdateTimeText();
    }

    // Call this ONCE when first ball is hit or thrown
    public void StartTimer()
    {
        if (timerRunning) return;
        timerRunning = true;
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();

        if (score > best)
        {
            best = score;
            UpdateBestText();
        }
    }

    void UpdateTimeText()
    {
        timeText.text = Mathf.Ceil(timeRemaining).ToString("00");
    }

    void UpdateScoreText()
    {
        scoreText.text = score.ToString("00");
    }

    void UpdateBestText()
    {
        bestText.text = best.ToString("00");
    }

}
