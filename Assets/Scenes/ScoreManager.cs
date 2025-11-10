using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("UI")]
    public Text scoreText;       // スコア表示
    public Text comboText;       // コンボ表示（Canvas上にTextを追加）

    private int score = 0;       // 現在のスコア
    private int comboCount = 0;  // 現在のコンボ数
    private float comboTimer = 0f; // コンボが続く残り時間
    private float comboDuration = 2.0f; // コンボ継続時間（秒）

    private float comboMultiplier = 1.0f; // スコア倍率

    void Start()
    {
        UpdateScoreText();
        UpdateComboText();
    }

    void Update()
    {
        // コンボの残り時間カウントダウン
        if (comboCount > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                comboCount = 0;
                comboMultiplier = 1.0f;
                UpdateComboText();
            }
        }
    }

    // 🔸 スコアを増減
    public void AddScore(int amount)
    {
        // 🔸 スコア0（=時間加算など）はコンボ維持
        if (amount < 0)
        {
            // 減点的のときだけリセット
            comboCount = 0;
            comboMultiplier = 1.0f;
            UpdateComboText();
        }
        else if (amount > 0)
        {
            // コンボ継続
            comboCount++;
            comboTimer = comboDuration;

            // コンボ倍率計算
            if (comboCount >= 5)
                comboMultiplier = 2.0f;
            else if (comboCount >= 3)
                comboMultiplier = 1.5f;
            else if (comboCount >= 2)
                comboMultiplier = 1.2f;
            else
                comboMultiplier = 1.0f;
        }

        // 最終加算スコア
        int finalAdd = Mathf.RoundToInt(amount * comboMultiplier);
        score += finalAdd;

        // スコアが0未満にならないように
        if (score < 0)
            score = 0;

        UpdateScoreText();
        UpdateComboText();

        Debug.Log($"スコア +{finalAdd}（コンボ: {comboCount}, 倍率: {comboMultiplier}）");
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    private void UpdateComboText()
    {
        if (comboText == null) return;

        if (comboCount <= 1)
        {
            comboText.text = "";
        }
        else
        {
            comboText.text = "COMBO x" + comboCount + "  (" + comboMultiplier.ToString("F1") + "x)";
        }
    }

    // 現在のコンボ数を取得
    public int GetComboCount()
    {
        return comboCount;
    }

    // 現在の倍率を取得
    public float GetComboMultiplier()
    {
        return comboMultiplier;
    }
}
