using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum TargetType
{
    Normal,
    Rare,
    Bad,
    TimePlus,
    TimeMinus
}

public class Target : MonoBehaviour, IPointerClickHandler
{
    // =========================
    // ■ 設定（Inspector）
    // =========================

    [Header("寿命（秒）")]
    public float minLifeTime = 2f;
    public float maxLifeTime = 5f;

    [Header("的のサイズ")]
    public float minScale = 0.6f;
    public float maxScale = 1.2f;

    [Header("レア度係数")]
    [Range(1f, 5f)]
    public float rarityPower = 1.0f;

    [Header("寿命倍率")]
    [Range(0.1f, 1f)]
    public float rareLifeMultiplier = 0.3f;

    [Header("スコア")]
    public int normalScore = 10;
    public int rareScore = 30;
    public int badScore = -40;

    [Header("表示色")]
    public Color normalColor = Color.gray;
    public Color rareColor = Color.red;
    public Color badColor = Color.blue;

    [Header("演出")]
    public GameObject hitParticlePrefab;
    public GameObject floatingTextPrefab;
    public AudioSource hitSE;

    [Header("参照")]
    public Camera mainCamera;
    public Canvas canvas;

    public TargetType targetType = TargetType.Normal;

    // =========================
    // ■ 内部変数
    // =========================

    private Image image;
    private float lifeTime;

    // =========================
    // ■ 初期化
    // =========================

    void Awake()
    {
        image = GetComponent<Image>();
        hitSE = GetComponent<AudioSource>();

        canvas = FindObjectOfType<Canvas>();
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Start()
    {
        SetupScale();
        ApplyVisualState();
        SetupLifeTime();
    }

    // =========================
    // ■ 初期設定
    // =========================

    void SetupScale()
    {
        float t = Mathf.Pow(Random.value, rarityPower);
        float scale = Mathf.Lerp(minScale, maxScale, t);
        transform.localScale = new Vector3(scale, scale, 1f);
    }

    void SetupLifeTime()
    {
        lifeTime = Random.Range(minLifeTime, maxLifeTime);

        if (targetType == TargetType.Rare)
            lifeTime *= rareLifeMultiplier;

        Destroy(gameObject, lifeTime);
    }

    void ApplyVisualState()
    {
        if (image == null) return;

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

    // =========================
    // ■ クリック処理
    // =========================

    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySE();

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
        text = BuildComboText(sm, addPoint, text);

        ShowFloatingText(text, color);
        ShowHitParticle();

        Destroy(gameObject, 0.2f);
    }

    // =========================
    // ■ 表示系
    // =========================

    string BuildComboText(ScoreManager sm, int addPoint, string baseText)
    {
        if (sm == null || addPoint <= 0) return baseText;

        int combo = sm.GetComboCount();
        float mul = sm.GetComboMultiplier();

        if (combo > 1)
            return $"+{addPoint} (x{mul:F1} COMBO {combo})";

        return baseText;
    }

    void ShowFloatingText(string text, Color color)
    {
        if (floatingTextPrefab == null || canvas == null) return;

        GameObject obj = Instantiate(floatingTextPrefab, canvas.transform);
        obj.transform.position = transform.position;

        var ctrl = obj.GetComponent<FloatingTextController>();
        ctrl?.Show(text, color);
    }

    void ShowHitParticle()
    {
        if (hitParticlePrefab == null || mainCamera == null) return;

        Vector3 worldPos = transform.position;
        worldPos.z = -9f;

        Instantiate(hitParticlePrefab, worldPos, Quaternion.identity);
    }

    void PlaySE()
    {
        if (hitSE != null && hitSE.clip != null)
            AudioSource.PlayClipAtPoint(hitSE.clip, mainCamera.transform.position);
    }
}