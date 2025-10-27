using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;   // CanvasのScoreオブジェクトをここに入れる
    private int score = 0;   // 現在のスコア

    void Start()
    {
        UpdateScoreText();
    }

    // スコアを増やす
    public void AddScore(int amount)
    {
        score += amount;

        // 🔸 スコアが0未満にならないようにする
        if (score < 0)
            score = 0;

        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}
