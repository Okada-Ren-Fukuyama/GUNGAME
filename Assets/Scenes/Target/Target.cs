using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum TargetType
{
    Normal,  // 通常的（+10点）
    Rare,    // レア的（+30点）
    Bad,      // マイナス的（−40点）
    TimePlus,   // ⏱時間追加
    TimeMinus   // ⏱時間減少
}

public class Target : MonoBehaviour, IPointerClickHandler
{
    [Header("寿命（秒）")]
    public float minLifeTime = 2f;
    public float maxLifeTime = 5f;

    [Header("的のサイズ")]
    public float minScale = 0.6f;
    public float maxScale = 1.2f;

    [Header("小さい的が出にくくなる強さ")]
    [Range(1f, 5f)]
    public float rarityPower = 1.0f;

    [Header("レア的のしきい値（これ以下ならレア色に）")]
    public float rareThreshold = 0.75f;

    [Header("通常の色")]
    public Color normalColor = Color.gray;

    [Header("レア的の色")]
    public Color rareColor = Color.red;

    [Header("マイナス的の色")]
    public Color badColor = Color.blue;

    [Header("レア的の寿命を短くする倍率（0.5 = 半分の時間）")]
    [Range(0.1f, 1f)]
    public float rareLifeMultiplier = 0.3f;

    [Header("当たりエフェクト（パーティクル）")]
    public GameObject hitParticlePrefab;
    public Camera mainCamera;

    public int normalScore = 10;   // 通常的の得点
    public int rareScore = 30;     // レア的のスコア
    public int badScore = -40;     // マイナス的のスコア

    public TargetType targetType = TargetType.Normal;

    private Image image;
    private float lifeTime;
    private float scale;
    public GameObject floatingTextPrefab;
    public Canvas canvas;

    void Start()
    {

        image = GetComponent<Image>();

        // サイズ決定（小さいほどレアっぽくなる）
        float t = Random.value;
        t = Mathf.Pow(t, rarityPower);
        scale = Mathf.Lerp(minScale, maxScale, t);
        transform.localScale = new Vector3(scale, scale, 1f);

        ApplyVisualState();

        // 寿命設定
        lifeTime = Random.Range(minLifeTime, maxLifeTime);
        if (targetType == TargetType.Rare)
            lifeTime *= rareLifeMultiplier;

        Destroy(gameObject, lifeTime);
    }

    void Awake()
    {
        // Canvas 自動取得
        canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
            Debug.LogError("❌ Canvas がシーンに見つかりません");

        // MainCamera 自動取得
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    // --- 色を更新する ---
    private void ApplyVisualState()
    {
        if (image == null) image = GetComponent<Image>();
        if (image != null)
        {
            switch (targetType)
            {
                case TargetType.Normal:
                    image.color = normalColor;
                    break;
                case TargetType.Rare:
                    image.color = rareColor;
                    break;
                case TargetType.Bad:
                    image.color = badColor;
                    break;

            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"ターゲットクリック！タイプ: {targetType}");
        Debug.Log("🎯 OnPointerClickが呼ばれた！");

        ScoreManager sm = FindObjectOfType<ScoreManager>();
        TimerController timer = FindObjectOfType<TimerController>();

        int addPoint = 0;
        string text = "";
        Color color = Color.white;

        switch (targetType)
        {
            case TargetType.Normal:
                addPoint = normalScore;
                text = "+" + normalScore;
                color = Color.yellow;
                break;

            case TargetType.Rare:
                addPoint = rareScore;
                text = "+" + rareScore;
                color = Color.magenta;
                break;

            case TargetType.Bad:
                addPoint = badScore;
                text = badScore.ToString();
                color = Color.red;
                break;

            case TargetType.TimePlus:
                timer?.AddTime(3f);
                text = "+3s";
                color = Color.green;
                break;

            case TargetType.TimeMinus:
                timer?.ReduceTime(3f);
                text = "-3s";
                color = Color.cyan;
                break;
        }

        sm?.AddScore(addPoint);
        if (sm != null && addPoint > 0)
        {
            int comboCount = sm.GetComboCount();
            float multiplier = sm.GetComboMultiplier();

            if (comboCount > 1)
                text = $"+{addPoint} (x{multiplier:F1} COMBO {comboCount})";
            else
                text = $"+{addPoint}";
        }
        else if (addPoint < 0)
        {
            text = addPoint.ToString();
        }
        ShowFloatingText(text, color);
        ShowHitParticle();

        Destroy(gameObject);
    }

    // ✅ OnPointerClickの外に書く
    void ShowFloatingText(string text, Color color)
    {
        if (floatingTextPrefab == null)
        {
            Debug.LogError("❌ floatingTextPrefab が未設定です（Inspectorで指定して）");
            return;
        }

        if (canvas == null)
        {
            Debug.LogError("❌ Canvas が見つかりません！FindObjectOfType 失敗");
            return;
        }

        GameObject obj = Instantiate(floatingTextPrefab, canvas.transform);
        obj.transform.position = transform.position;

        var ctrl = obj.GetComponent<FloatingTextController>();
        if (ctrl != null)
            ctrl.Show(text, color);
    }

    void ShowHitParticle()
    {
        if (hitParticlePrefab == null)
        {
            Debug.LogError("❌ hitParticlePrefab がセットされていません（Inspectorに設定して）");
            return;
        }

        if (mainCamera == null)
            mainCamera = Camera.main;

        // UI座標 → ワールド座標
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(transform.position);
        worldPos.z = 0f;

        Instantiate(hitParticlePrefab, worldPos, Quaternion.identity);
    }

   
}






