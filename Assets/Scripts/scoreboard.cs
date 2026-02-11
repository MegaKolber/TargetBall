using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    public TextMeshPro timeText;
    public TextMeshPro scoreText;
    public TextMeshPro bestText;

    [Header("Timer")]
    public float roundTime = 60f;

    float timeRemaining;
    bool timerRunning = false;

    int best = 0;

    void Start()
    {
        timeRemaining = roundTime;
        UpdateTimeText();
        UpdateScoreText();
        UpdateBestText();
    }

    void Update()
    {
        if (timerRunning)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                timerRunning = false;
                // end round logic can go here later
            }
        }

        UpdateTimeText();
        UpdateScoreText();
        UpdateBestText();
    }

    // Called ONCE from BallController on first hit
    public void StartTimer()
    {
        if (timerRunning) return;
        timerRunning = true;
    }

    void UpdateTimeText()
    {
        timeText.text = Mathf.Ceil(timeRemaining).ToString("00");
    }

    void UpdateScoreText()
    {
        int score = GameManager.Instance.GetScore();
        scoreText.text = score.ToString("00");

        if (score > best)
            best = score;
    }

    void UpdateBestText()
    {
        bestText.text = best.ToString("00");
    }
}
