using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    [Header("レア的の寿命を短くする倍率（0.5 = 半分の時間）")]
    [Range(0.1f, 1f)]
    public float rareLifeMultiplier = 0.4f;

    private Image image;
    private float lifeTime;
    private float scale;
    private bool isRare;

    void Start()
    {
        image = GetComponent<Image>();

        // サイズ決定（小さいほどレアっぽくなる）
        float t = Random.value;
        t = Mathf.Pow(t, rarityPower);
        scale = Mathf.Lerp(minScale, maxScale, t);
        transform.localScale = new Vector3(scale, scale, 1f);

        // デフォルトではサイズからレア判定（Spawnerから上書き可）
        isRare = scale <= rareThreshold;
        ApplyVisualState();

        // 寿命設定
        lifeTime = Random.Range(minLifeTime, maxLifeTime);
        if (isRare)
            lifeTime *= rareLifeMultiplier;

        Destroy(gameObject, lifeTime);
    }

    // 🔸 外部（Spawner）からレアを指定できるようにする
    public void SetRare(bool rare)
    {
        isRare = rare;
        ApplyVisualState();
    }

    // 🔸 レアかどうかを取得
    public bool IsRare() => isRare;

    // 🔸 強制的に通常状態にする
    public void ForceNormal()
    {
        isRare = false;
        ApplyVisualState();
    }

    // 🔸 色を反映（共通処理）
    private void ApplyVisualState()
    {
        if (image == null) image = GetComponent<Image>();
        if (image != null)
            image.color = isRare ? rareColor : normalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("ターゲットクリック！");
        Destroy(gameObject);
    }
}





