using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSpawner : MonoBehaviour
{
    [Header("的のPrefab")]
    public GameObject targetPrefab;

    [Header("的を出す範囲(UIのRectTransform)")]
    public RectTransform targetArea;

    [Header("生成間隔の最小/最大(秒)")]
    public float minSpawnInterval = 1.0f;
    public float maxSpawnInterval = 2.5f;

    [Header("一度に出す的の最小数")]
    public int minSpawnCount = 2;

    [Header("一度に出す的の最大数")]
    public int maxSpawnCount = 5;

    [Header("エリアの余白（左右・上下の調整）")]
    public float marginX = 100f;
    public float marginY = 80f;

    [Header("的の間隔（被り防止）")]
    public float minDistance = 120f;

    [Header("レア的の出現確率(0〜1)")]
    [Range(0f, 1f)] public float rareChance = 0.1f;

    [Header("マイナス的の出現確率(0〜1)")]
    [Range(0f, 1f)] public float badChance = 0.3f;

    private List<GameObject> spawnedTargets = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnTarget();

            float wait = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(wait);
        }
    }

    void SpawnTarget()
    {
        spawnedTargets.RemoveAll(t => t == null);

        int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 randomPos = GetSafeRandomPosition();
            if (randomPos == Vector2.zero) continue;

            GameObject target = Instantiate(targetPrefab, targetArea);
            target.transform.position = randomPos;
            spawnedTargets.Add(target);

            // --- タイプ判定 ---
            float roll = Random.value;
            Image img = target.GetComponent<Image>();
            Target t = target.GetComponent<Target>();

            if (img != null && t != null)
            {
                if (roll < badChance)
                {
                    img.color = Color.blue;   // 減点的
                    t.targetType = TargetType.Bad;
                }
                else if (roll < badChance + rareChance)
                {
                    img.color = Color.red;    // レア的
                    t.targetType = TargetType.Rare;
                }
                else
                {
                    img.color = Color.gray;   // 通常的
                    t.targetType = TargetType.Normal;
                }
            }
        }
    }

    Vector2 GetSafeRandomPosition()
    {
        Vector2 areaSize = targetArea.rect.size;
        for (int i = 0; i < 20; i++)
        {
            float x = Random.Range(-areaSize.x / 2 + marginX, areaSize.x / 2 - marginX);
            float y = Random.Range(-areaSize.y / 2 + marginY, areaSize.y / 2 - marginY);
            Vector2 localPos = new Vector2(x, y);
            Vector2 worldPos = targetArea.TransformPoint(localPos);

            bool tooClose = false;
            foreach (var t in spawnedTargets)
            {
                if (t == null) continue;
                float dist = Vector2.Distance(worldPos, t.transform.position);
                if (dist < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
                return worldPos;
        }

        return Vector2.zero;
    }

    void Update()
    {
        spawnedTargets.RemoveAll(t => t == null);
    }
}





