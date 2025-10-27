using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum TargetType
{
    Normal,  // 通常的（+10点）
    Rare,    // レア的（+30点）
    Bad      // マイナス的（−40点）
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

    public int normalScore = 10;   // 通常的の得点
    public int rareScore = 30;     // レア的のスコア
    public int badScore = -40;     // マイナス的のスコア

    public TargetType targetType = TargetType.Normal;

    private Image image;
    private float lifeTime;
    private float scale;

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

        // スコア加算／減点処理
        ScoreManager sm = FindObjectOfType<ScoreManager>();
        if (sm != null)
        {
            int addPoint = 0;
            switch (targetType)
            {
                case TargetType.Normal:
                    addPoint = normalScore;
                    break;
                case TargetType.Rare:
                    addPoint = rareScore;
                    break;
                case TargetType.Bad:
                    addPoint = badScore;
                    break;
            }

            sm.AddScore(addPoint);
        }

        Destroy(gameObject);
    }
}






