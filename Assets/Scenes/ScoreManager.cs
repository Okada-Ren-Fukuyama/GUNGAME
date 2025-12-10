using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // =========================
    // ■ UI
    // =========================

    [Header("UI")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text comboText;
    [SerializeField] private Text levelText;

    // =========================
    // ■ スコア・コンボ管理
    // =========================

    private int score = 0;
    private int comboCount = 0;

    private float comboTimer = 0f;
    [SerializeField] private float comboDuration = 2.0f;

    private float comboMultiplier = 1.0f;

    // =========================
    // ■ レベル管理
    // =========================

    [Header("レベル管理")]
    [SerializeField] private int level = 1;

    [SerializeField]
    private int[] levelTargets = new int[]
    {
        0,    // ダミー
        100,  // Lv1 → Lv2
        250,  // Lv2 → Lv3
        500,  // Lv3 → Lv4
        900   // Lv4 → Lv5
    };

    // =========================
    // ■ 他スクリプト参照
    // =========================

    [Header("外部参照")]
    [SerializeField] private TargetSpawner targetSpawner;
    [SerializeField] private UIEffectManager uiEffectManager;

    // =========================
    // ■ 初期化
    // =========================

    void Start()
    {
        UpdateScoreText();
        UpdateComboText();
        UpdateLevelText();

        if (targetSpawner != null)
        {
            targetSpawner.SetDifficultyByLevel(level);
        }
        else
        {
            Debug.LogWarning("ScoreManager：TargetSpawner が設定されていません");
        }
    }

    // =========================
    // ■ 更新処理
    // =========================

    void Update()
    {
        UpdateComboTimer();
    }

    void UpdateComboTimer()
    {
        if (comboCount <= 0) return;

        comboTimer -= Time.deltaTime;

        if (comboTimer <= 0f)
        {
            comboCount = 0;
            comboMultiplier = 1.0f;
            UpdateComboText();
        }
    }

    // =========================
    // ■ スコア加算
    // =========================

    public void AddScore(int amount)
    {
        UpdateCombo(amount);

        int finalAdd = Mathf.RoundToInt(amount * comboMultiplier);
        score += finalAdd;

        if (score < 0)
            score = 0;

        UpdateScoreText();
        UpdateComboText();
        CheckLevelUp();

        Debug.Log($"スコア +{finalAdd}（コンボ: {comboCount}, 倍率: {comboMultiplier}）");
    }

    void UpdateCombo(int amount)
    {
        // 減点 → コンボリセット
        if (amount < 0)
        {
            comboCount = 0;
            comboMultiplier = 1.0f;
            return;
        }

        // 加点 → コンボ継続
        if (amount > 0)
        {
            comboCount++;
            comboTimer = comboDuration;

            if (comboCount >= 15)
                comboMultiplier = 2.0f;
            else if (comboCount >= 10)
                comboMultiplier = 1.5f;
            else if (comboCount >= 5)
                comboMultiplier = 1.2f;
            else
                comboMultiplier = 1.0f;
        }
    }

    // =========================
    // ■ レベルアップ判定
    // =========================

    void CheckLevelUp()
    {
        if (level >= levelTargets.Length) return;

        if (score >= levelTargets[level])
        {
            level++;
            Debug.Log("レベルアップ！ 現在Lv：" + level);

            UpdateLevelText();

            if (targetSpawner != null)
            {
                targetSpawner.SetDifficultyByLevel(level);
            }
            else
            {
                Debug.LogError("ScoreManager：targetSpawner が未設定です！");
            }

            if (uiEffectManager != null)
            {
                uiEffectManager.ShowLevelUp("LEVEL UP!");
            }
            else
            {
                Debug.LogWarning("ScoreManager：UIEffectManager が未設定です！");
            }
        }
    }

    // =========================
    // ■ UI更新
    // =========================

    void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void UpdateComboText()
    {
        if (comboText == null) return;

        if (comboCount <= 1)
            comboText.text = "";
        else
            comboText.text = $"COMBO x{comboCount}  ({comboMultiplier:F1}x)";
    }

    void UpdateLevelText()
    {
        if (levelText != null)
            levelText.text = "Lv : " + level;
    }

    // =========================
    // ■ 外部取得用（Getter）
    // =========================

    public int GetScore() => score;
    public int GetComboCount() => comboCount;
    public float GetComboMultiplier() => comboMultiplier;
}
