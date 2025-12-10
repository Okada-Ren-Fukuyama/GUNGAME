using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSpawner : MonoBehaviour
{
    [Header("Prefab / Area")]
    public GameObject targetPrefab;
    public RectTransform targetArea;

    [Header("Spawn Interval")]
    public float minSpawnInterval = 1.0f;
    public float maxSpawnInterval = 2.5f;

    [Header("Spawn Amount")]
    public int minSpawnCount = 3;
    public int maxSpawnCount = 4;

    [Header("Area Margin")]
    public float marginX = 100f;
    public float marginY = 80f;

    [Header("Distance Limit")]
    public float minDistance = 120f;

    [Header("Target Rates")]
    [Range(0f, 1f)] public float rareChance = 0.1f;
    [Range(0f, 1f)] public float badChance = 0.2f;

    [Header("External References")]
    public UIEffectManager uiEffectManager;

    [Header("Game State")]
    public bool isGameStarted = false;

    private List<GameObject> spawnedTargets = new List<GameObject>();
    private TimerController timer;

    // ----------------------------
    // 初期化
    // ----------------------------
    void Awake()
    {
        timer = FindObjectOfType<TimerController>();
        isGameStarted = false;

        StartCoroutine(SpawnLoop());
    }

    // ----------------------------
    // ゲーム開始
    // ----------------------------
    public void StartGame()
    {
        isGameStarted = true;
        Debug.Log("★ Game Started");
    }

    // ----------------------------
    // スポーンループ
    // ----------------------------
    IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (!isGameStarted)
            {
                yield return null;
                continue;
            }

            if (timer != null && timer.IsTimeUp())
            {
                yield break;
            }

            SpawnTarget();

            float wait = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(wait);
        }
    }

    // ----------------------------
    // 的スポーン処理
    // ----------------------------
    void SpawnTarget()
    {
        spawnedTargets.RemoveAll(t => t == null);

        int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 randomPos = GetSafeRandomPosition();
            if (randomPos == Vector2.zero) continue;

            GameObject target = Instantiate(targetPrefab, targetArea);
            target.GetComponent<RectTransform>().localPosition = randomPos;
            spawnedTargets.Add(target);

            SetTargetType(target);
        }
    }

    // ----------------------------
    // 的タイプ設定
    // ----------------------------
    void SetTargetType(GameObject target)
    {
        float roll = Random.value;
        Image img = target.GetComponent<Image>();
        Target tg = target.GetComponent<Target>();

        if (img == null || tg == null) return;

        if (roll < badChance)
        {
            img.color = Color.blue;
            tg.targetType = TargetType.Bad;
        }
        else if (roll < badChance + rareChance)
        {
            img.color = Color.red;
            tg.targetType = TargetType.Rare;
        }
        else if (roll < badChance + rareChance + 0.1f)
        {
            img.color = Color.cyan;
            tg.targetType = TargetType.TimeMinus;
        }
        else if (roll < badChance + rareChance + 0.2f)
        {
            img.color = Color.green;
            tg.targetType = TargetType.TimePlus;
        }
        else
        {
            img.color = Color.gray;
            tg.targetType = TargetType.Normal;
        }
    }

    // ----------------------------
    // 安全なランダム位置取得
    // ----------------------------
    Vector2 GetSafeRandomPosition()
    {
        Vector2 areaSize = targetArea.rect.size;

        for (int i = 0; i < 20; i++)
        {
            float x = Random.Range(-areaSize.x / 2 + marginX, areaSize.x / 2 - marginX);
            float y = Random.Range(-areaSize.y / 2 + marginY, areaSize.y / 2 - marginY);
            Vector2 localPos = new Vector2(x, y);

            bool tooClose = false;

            foreach (var t in spawnedTargets)
            {
                if (t == null) continue;

                Vector2 tLocal = t.GetComponent<RectTransform>().localPosition;
                if (Vector2.Distance(localPos, tLocal) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
                return localPos;
        }

        return Vector2.zero;
    }

    // ----------------------------
    // レベルごとの難易度設定（表示は別スクリプト）
    // ----------------------------
    public void SetDifficultyByLevel(int level)
    {
        if (level == 1)
        {
            minSpawnInterval = 1.2f;
            maxSpawnInterval = 2.5f;
            minSpawnCount = 2;
            maxSpawnCount = 4;
            rareChance = 0.1f;
            badChance = 0.2f;
        }
        else if (level == 2)
        {
            minSpawnInterval = 0.9f;
            maxSpawnInterval = 1.8f;
            minSpawnCount = 3;
            maxSpawnCount = 5;
            rareChance = 0.15f;
            badChance = 0.25f;

            if (uiEffectManager != null)
                uiEffectManager.ShowLevelUp("LEVEL UP!");
        }
        else if (level == 3)
        {
            minSpawnInterval = 0.6f;
            maxSpawnInterval = 1.2f;
            minSpawnCount = 4;
            maxSpawnCount = 7;
            rareChance = 0.2f;
            badChance = 0.3f;

            if (uiEffectManager != null)
                uiEffectManager.ShowLevelUp("LEVEL UP!");
        }
        else if (level == 4)
        {
            minSpawnInterval = 0.4f;
            maxSpawnInterval = 0.9f;
            minSpawnCount = 5;
            maxSpawnCount = 9;
            rareChance = 0.3f;
            badChance = 0.4f;

            if (uiEffectManager != null)
                uiEffectManager.ShowLevelUp("LEVEL UP!!!");
        }
    }
}





